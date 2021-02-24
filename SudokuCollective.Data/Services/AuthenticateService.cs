using System;
using System.Collections.Generic;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using SudokuCollective.Core.Interfaces.APIModels.TokenModels;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.TokenModels;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Data.Messages;
using System.Linq;
using SudokuCollective.Core.Enums;

namespace SudokuCollective.Data.Services
{
    public class AuthenticateService : IAuthenticateService
    {
        private readonly IUsersRepository<User> usersRepository;
        private readonly IRolesRepository<Role> rolesRepository;
        private readonly IAppsRepository<App> appsRepository;
        private readonly IAppAdminsRepository<AppAdmin> appAdminsRepository;
        private readonly IUserManagementService userManagementService;
        private readonly ITokenManagement tokenManagement;

        public AuthenticateService(
            IUsersRepository<User> usersRepo,
            IRolesRepository<Role> rolesRepo,
            IAppsRepository<App> appsRepo,
            IAppAdminsRepository<AppAdmin> appsAdminRepo,
            IUserManagementService userManagementServ,
            IOptions<TokenManagement> tokenManage)
        {
            usersRepository = usersRepo;
            rolesRepository = rolesRepo;
            appsRepository = appsRepo;
            appAdminsRepository = appsAdminRepo;
            userManagementService = userManagementServ;
            tokenManagement = tokenManage.Value;
        }

        async public Task<IAuthenticatedUserResult> IsAuthenticated(ITokenRequest request)
        {
            var result = new AuthenticatedUserResult();

            var validateUserTask = userManagementService.IsValidUser(request.UserName, request.Password);

            validateUserTask.Wait();

            if (!validateUserTask.Result)
            {
                result.Success = false;
                result.Message = UsersMessages.UserNotFoundMessage;

                return result;
            }

            var user = (User)(await usersRepository.GetByUserName(request.UserName, true)).Object;

            var app = (App)(await appsRepository.GetByLicense(request.License)).Object;

            var appAdmins = (await appAdminsRepository.GetAll()).Objects.ConvertAll(aa => (AppAdmin)aa);

            if (app.Id != 1)
            {
                if (!user.IsSuperUser)
                {
                    if (user.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                    {
                        if (!appAdmins.Any(aa => aa.AppId == app.Id && aa.UserId == user.Id))
                        {
                            var adminRole = user
                                .Roles
                                .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.ADMIN);

                            user.Roles.Remove(adminRole);
                        }
                    }
                }
            }

            result.User.UpdateWithUserInfo(user);

            var claim = new List<Claim> {

                new Claim(ClaimTypes.Name, request.UserName)
            };

            foreach (var role in user.Roles)
            {
                var r = (Role)(await rolesRepository.GetById(role.Role.Id)).Object;

                claim.Add(new Claim(ClaimTypes.Role, r.RoleLevel.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenManagement.Secret));

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var jwtToken = new JwtSecurityToken(
                    tokenManagement.Issuer,
                    tokenManagement.Audience,
                    claim.ToArray(),
                    expires: DateTime.UtcNow.AddHours(tokenManagement.AccessExpiration),
                    signingCredentials: credentials
                );

            result.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            result.Success = true;
            result.Message = UsersMessages.UserFoundMessage;

            return result;
        }
    }
}
