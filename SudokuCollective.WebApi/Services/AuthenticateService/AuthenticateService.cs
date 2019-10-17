using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using SudokuCollective.WebApi.Models.TokenModels;

namespace SudokuCollective.WebApi.Services {

    public class AuthenticateService : IAuthenticateService {

        private readonly ApplicationDbContext _context;
        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagement _tokenManagement;

        public AuthenticateService(
            ApplicationDbContext context,
            IUserManagementService service,
            IOptions<TokenManagement> tokenManagment) {

            _context = context;
            _userManagementService = service;
            _tokenManagement = tokenManagment.Value;
        }

        public bool IsAuthenticated(TokenRequest request, out string token) {

            token = string.Empty;

            var validateUserTask = _userManagementService.IsValidUser(request.UserName, request.Password);
            validateUserTask.Wait();

            if (!validateUserTask.Result) {

                return false;
            }
            
            var user = _context.Users
                .Where(u => u.UserName.Equals(request.UserName))
                .Include(u => u.Roles)
                .FirstOrDefault();

            var claim = new List<Claim> {

                new Claim(ClaimTypes.Name, request.UserName)
            };

            foreach (var role in user.Roles) {

                var r = _context.Roles.Where(x => x.Id == role.RoleId).FirstOrDefault();
                
                claim.Add(new Claim(ClaimTypes.Role, r.RoleLevel.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                    _tokenManagement.Issuer,
                    _tokenManagement.Audience,
                    claim.ToArray(),
                    expires:DateTime.UtcNow.AddMinutes(_tokenManagement.AccessExpiration),
                    signingCredentials: credentials
                );

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}
