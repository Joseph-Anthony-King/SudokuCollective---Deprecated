using System;
using System.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SudokuApp.WebApp.Models;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Services {

    public class TokenAuthenticationService : IAuthenticateService {

        private readonly ApplicationDbContext _context;
        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(
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
            
            var user = _context.Users.Where(u => u.UserName.Equals(request.UserName)).FirstOrDefault();

            var claim = new[] {

                new Claim(ClaimTypes.Name, request.UserName)
            };

            var i = 1;

            foreach (var role in user.Roles) {
                
                claim[i] = new Claim(ClaimTypes.Role, role.Role.RoleLevel.ToString());
                i++;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                    _tokenManagement.Issuer,
                    _tokenManagement.Audience,
                    claim,
                    expires:DateTime.UtcNow.AddMinutes(_tokenManagement.AccessExpiration),
                    signingCredentials: credentials
                );

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}
