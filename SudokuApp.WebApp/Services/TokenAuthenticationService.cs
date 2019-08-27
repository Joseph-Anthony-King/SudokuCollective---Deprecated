using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SudokuApp.WebApp.Models;
using SudokuApp.WebApp.Services.Interfaces;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SudokuApp.WebApp.Services {

    public class TokenAuthenticationService : IAuthenticateService {

        private readonly IUserManagementService _userManagementService;
        private readonly TokenManagement _tokenManagement;

        public TokenAuthenticationService(
            IUserManagementService service,
            IOptions<TokenManagement> tokenManagment) {

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

            var claim = new[] {

                new Claim(ClaimTypes.Name, request.UserName)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenManagement.Secret));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                    _tokenManagement.Issuer,
                    _tokenManagement.Audience,
                    claim,
                    expires:DateTime.Now.AddMinutes(_tokenManagement.AccessExpiration),
                    signingCredentials: credentials
                );

            token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            return true;
        }
    }
}
