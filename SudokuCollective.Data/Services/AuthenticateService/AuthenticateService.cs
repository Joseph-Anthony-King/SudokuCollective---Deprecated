using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.APIModels.DTOModels;
using SudokuCollective.Core.Interfaces.APIModels.TokenModels;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DTOModels;
using SudokuCollective.Data.Models.TokenModels;

namespace SudokuCollective.Data.Services
{
    public class AuthenticateService : IAuthenticateService
    {

        private readonly DatabaseContext _context;
        private readonly IUserManagementService _userManagementService;
        private readonly ITokenManagement _tokenManagement;

        public AuthenticateService(
            DatabaseContext context,
            IUserManagementService service,
            IOptions<TokenManagement> tokenManagment)
        {
            _context = context;
            _userManagementService = service;
            _tokenManagement = tokenManagment.Value;
        }

        public bool IsAuthenticated(ITokenRequest request, out string token, out IAuthenticatedUser user)
        {
            token = string.Empty;
            user = new AuthenticatedUser();

            var validateUserTask = _userManagementService.IsValidUser(request.UserName, request.Password);
            validateUserTask.Wait();

            if (!validateUserTask.Result)
            {
                return false;
            }

            var authenticatedUser = (User)_context.Users
                .Where(u => u.UserName.Equals(request.UserName))
                .Include(u => u.Roles)
                .FirstOrDefault();

            foreach (var role in authenticatedUser.Roles)
            {
                role.Role = _context.Roles.Where(x => x.Id == role.RoleId).FirstOrDefault();
            }

            user.UpdateWithUserInfo(authenticatedUser);

            // UpdateWithUserInfo works with an instance of IUser and has no knowledge
            // for user roles or the IsSuperUser or IsAdmin properites on which those
            // properties are based.  As such these values are set manually.
            user.IsSuperUser = authenticatedUser.IsSuperUser;
            user.IsAdmin = authenticatedUser.IsAdmin;

            var claim = new List<Claim> {

                new Claim(ClaimTypes.Name, request.UserName)
            };

            foreach (var role in authenticatedUser.Roles)
            {
                var r = _context.Roles.Where(x => x.Id == role.RoleId).FirstOrDefault();

                claim.Add(new Claim(ClaimTypes.Role, r.RoleLevel.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                    _tokenManagement.Issuer,
                    _tokenManagement.Audience,
                    claim.ToArray(),
                    expires: DateTime.UtcNow.AddMinutes(_tokenManagement.AccessExpiration),
                    signingCredentials: credentials
                );

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}
