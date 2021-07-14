using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<IAuthenticatedUserResult> IsAuthenticated(ITokenRequest request)
        {
            if (request == null) throw new ArgumentNullException(nameof(request));

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

            if (!app.IsActive)
            {
                result.Success = false;
                result.Message = AppsMessages.AppDeactivatedMessage;

                return result;
            }

            if (!app.PermitCollectiveLogins && !app.Users.Any(ua => ua.UserId == user.Id))
            {
                result.Success = false;
                result.Message = AppsMessages.UserIsNotARegisteredUserOfThisAppMessage;

                return result;
            }

            var appAdmins = (await appAdminsRepository.GetAll()).Objects.ConvertAll(aa => (AppAdmin)aa);

            if (!user.IsSuperUser)
            {
                if (user.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                {
                    if (!appAdmins.Any(aa => aa.AppId == app.Id && aa.UserId == user.Id && aa.IsActive))
                    {
                        var adminRole = user
                            .Roles
                            .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.ADMIN);

                        user.Roles.Remove(adminRole);
                    }
                }
            }
            else
            {
                if (!app.PermitSuperUserAccess)
                {
                    if (user.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER))
                    {
                        var superUserRole = user
                            .Roles
                            .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER);

                        user.Roles.Remove(superUserRole);
                    }

                    if (user.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                    {
                        var adminRole = user
                            .Roles
                            .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.ADMIN);

                        user.Roles.Remove(adminRole);
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

            DateTime expirationLimit;

            if (app.TimeFrame == TimeFrame.SECONDS)
            {
                expirationLimit = DateTime.UtcNow.AddSeconds(app.AccessDuration);
            }
            else if (app.TimeFrame == TimeFrame.MINUTES)
            {
                expirationLimit = DateTime.UtcNow.AddMinutes(app.AccessDuration);
            }
            else if (app.TimeFrame == TimeFrame.HOURS)
            {
                expirationLimit = DateTime.UtcNow.AddHours(app.AccessDuration);
            }
            else if (app.TimeFrame == TimeFrame.DAYS)
            {
                expirationLimit = DateTime.UtcNow.AddDays(app.AccessDuration);
            }
            else
            {
                expirationLimit = DateTime.UtcNow.AddMonths(app.AccessDuration);
            }

            var jwtToken = new JwtSecurityToken(
                    tokenManagement.Issuer,
                    tokenManagement.Audience,
                    claim.ToArray(),
                    notBefore: DateTime.UtcNow,
                    expires: expirationLimit,
                    signingCredentials: credentials
                );

            result.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            result.Success = true;
            result.Message = UsersMessages.UserFoundMessage;

            return result;
        }
    }
}
