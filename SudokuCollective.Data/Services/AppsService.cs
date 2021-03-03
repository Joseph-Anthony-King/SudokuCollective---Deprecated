using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Data.Services
{
    public class AppsService : IAppsService
    {
        #region Fields
        private readonly IAppsRepository<App> appsRepository;
        private readonly IUsersRepository<User> usersRepository;
        private readonly IAppAdminsRepository<AppAdmin> appAdminsRepository;
        private readonly IRolesRepository<Role> rolesRepository;
        #endregion

        #region Constructor
        public AppsService(
            IAppsRepository<App> appRepo, 
            IUsersRepository<User> userRepo,
            IAppAdminsRepository<AppAdmin> appAdminsRepo,
            IRolesRepository<Role> rolesRepo)
        {
            appsRepository = appRepo;
            usersRepository = userRepo;
            appAdminsRepository = appAdminsRepo;
            rolesRepository = rolesRepo;
        }
        #endregion

        #region Methods
        public async Task<IAppResult> GetApp(int id, bool fullRecord = true)
        {
            var result = new AppResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    var response = await appsRepository.GetById(id, fullRecord);

                    if (response.Success)
                    {
                        var app = (App)response.Object;

                        if (fullRecord)
                        {
                            var appAdmins = (await appAdminsRepository.GetAll())
                                .Objects
                                .ConvertAll(aa => (AppAdmin)aa)
                                .ToList();

                            foreach (var userApp in app.Users)
                            {
                                userApp.App = null;
                                userApp.User.Apps = new List<UserApp>();

                                if (app.Id != 1 && userApp
                                    .User
                                    .Roles
                                    .Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                {
                                    if (!userApp.User.IsSuperUser)
                                    {
                                        if (!appAdmins.Any(aa =>
                                            aa.AppId == app.Id &&
                                            aa.UserId == userApp.User.Id &&
                                            aa.IsActive))
                                        {
                                            var adminRole = userApp
                                                .User
                                                .Roles
                                                .FirstOrDefault(ur =>
                                                    ur.Role.RoleLevel == RoleLevel.ADMIN);

                                            userApp.User.Roles.Remove(adminRole);
                                        }
                                    }
                                    else
                                    {
                                        if (!app.PermitSuperUserAccess)
                                        {
                                            if (userApp.User.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER))
                                            {
                                                var superUserRole = userApp
                                                    .User
                                                    .Roles
                                                    .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER);

                                                userApp.User.Roles.Remove(superUserRole);
                                            }

                                            if (userApp.User.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                            {
                                                var adminRole = userApp
                                                    .User
                                                    .Roles
                                                    .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.ADMIN);

                                                userApp.User.Roles.Remove(adminRole);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        result.Success = response.Success;
                        result.Message = AppsMessages.AppFoundMessage;
                        result.App = app;

                        return result;
                    }
                    else if (!response.Success && response.Exception != null)
                    {
                        result.Success = response.Success;
                        result.Message = response.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = AppsMessages.AppNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IAppResult> GetAppByLicense(string license, bool fullRecord = true)
        {
            var result = new AppResult();

            try
            {
                if (await appsRepository.IsAppLicenseValid(license))
                {
                    var response = await appsRepository.GetByLicense(license, fullRecord);

                    if (response.Success)
                    {
                        var app = (IApp)response.Object;

                        if (fullRecord)
                        {
                            var appAdmins = (await appAdminsRepository.GetAll())
                                .Objects
                                .ConvertAll(aa => (AppAdmin)aa)
                                .ToList();

                            foreach (var userApp in app.Users)
                            {
                                userApp.App = null;
                                userApp.User.Apps = new List<UserApp>();

                                if (app.Id != 1 && userApp
                                    .User
                                    .Roles
                                    .Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                {
                                    if (!userApp.User.IsSuperUser)
                                    {
                                        if (!appAdmins.Any(aa =>
                                            aa.AppId == app.Id &&
                                            aa.UserId == userApp.User.Id &&
                                            aa.IsActive))
                                        {
                                            var adminRole = userApp
                                                .User
                                                .Roles
                                                .FirstOrDefault(ur =>
                                                    ur.Role.RoleLevel == RoleLevel.ADMIN);

                                            userApp.User.Roles.Remove(adminRole);
                                        }
                                    }
                                    else
                                    {
                                        if (!app.PermitSuperUserAccess)
                                        {
                                            if (userApp.User.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER))
                                            {
                                                var superUserRole = userApp
                                                    .User
                                                    .Roles
                                                    .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER);

                                                userApp.User.Roles.Remove(superUserRole);
                                            }

                                            if (userApp.User.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                            {
                                                var adminRole = userApp
                                                    .User
                                                    .Roles
                                                    .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.ADMIN);

                                                userApp.User.Roles.Remove(adminRole);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        result.Success = response.Success;
                        result.Message = AppsMessages.AppFoundMessage;
                        result.App = app;

                        return result;
                    }
                    else if (!response.Success && response.Exception != null)
                    {
                        result.Success = response.Success;
                        result.Message = response.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = AppsMessages.AppNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IAppsResult> GetApps(
            IPageListModel pageListModel,
            bool fullRecord = true)
        {
            var result = new AppsResult();

            try
            {
                var response = await appsRepository.GetAll(fullRecord);

                if (response.Success)
                {
                    if (pageListModel != null)
                    {
                        if (StaticDataHelpers.IsPageValid(pageListModel, response.Objects))
                        {
                            if (pageListModel.SortBy == SortValue.NULL)
                            {
                                result.Apps = response.Objects.ConvertAll(a => (IApp)a);
                            }
                            else if (pageListModel.SortBy == SortValue.ID)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderBy(a => a.Id)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderByDescending(a => a.Id)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.GAMECOUNT)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderBy(a => a.GameCount)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderByDescending(a => a.GameCount)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.USERCOUNT)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderBy(a => a.UserCount)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderByDescending(a => a.UserCount)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.NAME)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderBy(a => a.Name)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderByDescending(a => a.Name)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.DATECREATED)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderBy(a => a.DateCreated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderByDescending(a => a.DateCreated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.DATEUPDATED)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderBy(a => a.DateUpdated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .OrderByDescending(a => a.DateUpdated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = ServicesMesages.SortValueNotImplementedMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = ServicesMesages.PageNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Apps = response.Objects.ConvertAll(a => (IApp)a);
                    }

                    if (fullRecord)
                    {
                        var appAdmins = (await appAdminsRepository.GetAll())
                            .Objects
                            .ConvertAll(aa => (AppAdmin)aa)
                            .ToList();

                        foreach (var app in result.Apps)
                        {
                            foreach (var userApp in app.Users)
                            {
                                userApp.App = null;
                                userApp.User.Apps = new List<UserApp>();

                                if (app.Id != 1 && userApp
                                    .User
                                    .Roles
                                    .Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                {
                                    if (!userApp.User.IsSuperUser)
                                    {
                                        if (!appAdmins.Any(aa =>
                                            aa.AppId == app.Id &&
                                            aa.UserId == userApp.User.Id &&
                                            aa.IsActive))
                                        {
                                            var adminRole = userApp
                                                .User
                                                .Roles
                                                .FirstOrDefault(ur =>
                                                    ur.Role.RoleLevel == RoleLevel.ADMIN);

                                            userApp.User.Roles.Remove(adminRole);
                                        }
                                    }
                                    else
                                    {
                                        if (!app.PermitSuperUserAccess)
                                        {
                                            if (userApp.User.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER))
                                            {
                                                var superUserRole = userApp
                                                    .User
                                                    .Roles
                                                    .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER);

                                                userApp.User.Roles.Remove(superUserRole);
                                            }

                                            if (userApp.User.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                            {
                                                var adminRole = userApp
                                                    .User
                                                    .Roles
                                                    .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.ADMIN);

                                                userApp.User.Roles.Remove(adminRole);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    result.Success = response.Success;
                    result.Message = AppsMessages.AppsFoundMessage;

                    return result;
                }
                else if (!response.Success && response.Exception != null)
                {
                    result.Success = response.Success;
                    result.Message = response.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppsNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IAppsResult> GetMyApps(
            int ownerId, 
            IPageListModel pageListModel, 
            bool fullRecord = true)
        {
            var result = new AppsResult();

            try
            {
                var response = await appsRepository.GetAll(fullRecord);

                if (response.Success)
                {
                    if (pageListModel != null)
                    {
                        if (StaticDataHelpers.IsPageValid(pageListModel, response.Objects))
                        {
                            if (pageListModel.SortBy == SortValue.NULL)
                            {
                                result.Apps = response
                                    .Objects
                                    .ConvertAll(a => (IApp)a)
                                    .Where(a => a.OwnerId == ownerId)
                                    .ToList();
                            }
                            else if (pageListModel.SortBy == SortValue.ID)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderBy(a => a.Id)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderByDescending(a => a.Id)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.GAMECOUNT)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderBy(a => a.GameCount)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderByDescending(a => a.GameCount)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.USERCOUNT)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderBy(a => a.UserCount)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderByDescending(a => a.UserCount)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.NAME)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderBy(a => a.Name)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderByDescending(a => a.Name)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.DATECREATED)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderBy(a => a.DateCreated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderByDescending(a => a.DateCreated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else if (pageListModel.SortBy == SortValue.DATEUPDATED)
                            {
                                if (!pageListModel.OrderByDescending)
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderBy(a => a.DateUpdated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                                else
                                {
                                    foreach (var obj in response.Objects)
                                    {
                                        result.Apps.Add((IApp)obj);
                                    }

                                    result.Apps = result.Apps
                                        .Where(a => a.OwnerId == ownerId)
                                        .OrderByDescending(a => a.DateUpdated)
                                        .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                        .Take(pageListModel.ItemsPerPage)
                                        .ToList();
                                }
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = ServicesMesages.SortValueNotImplementedMessage;

                                return result;
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = ServicesMesages.PageNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Apps = response.Objects.ConvertAll(a => (IApp)a);
                    }

                    if (fullRecord)
                    {
                        var appAdmins = (await appAdminsRepository.GetAll())
                            .Objects
                            .ConvertAll(aa => (AppAdmin)aa)
                            .ToList();

                        foreach (var app in result.Apps)
                        {
                            foreach (var userApp in app.Users)
                            {
                                userApp.App = null;
                                userApp.User.Apps = new List<UserApp>();

                                if (app.Id != 1 && userApp
                                    .User
                                    .Roles
                                    .Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                {
                                    if (!userApp.User.IsSuperUser)
                                    {
                                        if (!appAdmins.Any(aa =>
                                            aa.AppId == app.Id &&
                                            aa.UserId == userApp.User.Id &&
                                            aa.IsActive))
                                        {
                                            var adminRole = userApp
                                                .User
                                                .Roles
                                                .FirstOrDefault(ur =>
                                                    ur.Role.RoleLevel == RoleLevel.ADMIN);

                                            userApp.User.Roles.Remove(adminRole);
                                        }
                                    }
                                    else
                                    {
                                        if (!app.PermitSuperUserAccess)
                                        {
                                            if (userApp.User.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER))
                                            {
                                                var superUserRole = userApp
                                                    .User
                                                    .Roles
                                                    .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER);

                                                userApp.User.Roles.Remove(superUserRole);
                                            }

                                            if (userApp.User.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                            {
                                                var adminRole = userApp
                                                    .User
                                                    .Roles
                                                    .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.ADMIN);

                                                userApp.User.Roles.Remove(adminRole);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    result.Success = response.Success;
                    result.Message = AppsMessages.AppsFoundMessage;

                    return result;
                }
                else if (!response.Success && response.Exception != null)
                {
                    result.Success = response.Success;
                    result.Message = response.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppsNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IUsersResult> GetAppUsers(
            int id,
            int requestorId,
            IPageListModel pageListModel,
            bool fullRecord = true)
        {
            var result = new UsersResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    var app = (App)(await appsRepository.GetById(id)).Object;
                    var response = await appsRepository.GetAppUsers(id, fullRecord);

                    if (response.Success)
                    {
                        if (pageListModel.SortBy == SortValue.NULL)
                        {
                            result.Users = response.Objects.ConvertAll(u => (IUser)u);
                        }
                        else if (pageListModel.SortBy == SortValue.ID)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.Id)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderByDescending(u => u.Id)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.USERNAME)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.UserName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderByDescending(u => u.UserName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.FIRSTNAME)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.FirstName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderByDescending(u => u.FirstName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.LASTNAME)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.LastName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderByDescending(u => u.LastName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.FULLNAME)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.FullName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderByDescending(u => u.FullName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.NICKNAME)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.NickName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderByDescending(u => u.NickName)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else if (pageListModel.SortBy == SortValue.GAMECOUNT)
                        {
                            if (!pageListModel.OrderByDescending)
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderBy(u => u.Games.Count)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                            else
                            {
                                foreach (var obj in response.Objects)
                                {
                                    result.Users.Add((IUser)obj);
                                }

                                result.Users = result.Users
                                    .OrderByDescending(u => u.Games.Count)
                                    .Skip((pageListModel.Page - 1) * pageListModel.ItemsPerPage)
                                    .Take(pageListModel.ItemsPerPage)
                                    .ToList();
                            }
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = ServicesMesages.SortValueNotImplementedMessage;

                            return result;
                        }

                        if (fullRecord)
                        {
                            var appAdmins = (await appAdminsRepository.GetAll())
                                .Objects
                                .ConvertAll(aa => (AppAdmin)aa)
                                .ToList();

                            foreach (var user in result.Users)
                            {
                                foreach (var userApp in user.Apps)
                                {
                                    userApp.App = null;
                                    userApp.User.Apps = new List<UserApp>();

                                    if (id != 1 && userApp
                                        .User
                                        .Roles
                                        .Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                    {
                                        if (!userApp.User.IsSuperUser)
                                        {
                                            if (!appAdmins.Any(aa =>
                                                aa.AppId == id &&
                                                aa.UserId == userApp.User.Id &&
                                                aa.IsActive))
                                            {
                                                var adminRole = userApp
                                                    .User
                                                    .Roles
                                                    .FirstOrDefault(ur =>
                                                        ur.Role.RoleLevel == RoleLevel.ADMIN);

                                                userApp.User.Roles.Remove(adminRole);
                                            }
                                        }
                                        else
                                        {
                                            if (!app.PermitSuperUserAccess)
                                            {
                                                if (userApp.User.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER))
                                                {
                                                    var superUserRole = userApp
                                                        .User
                                                        .Roles
                                                        .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.SUPERUSER);

                                                    userApp.User.Roles.Remove(superUserRole);
                                                }

                                                if (userApp.User.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                                                {
                                                    var adminRole = userApp
                                                        .User
                                                        .Roles
                                                        .FirstOrDefault(ur => ur.Role.RoleLevel == RoleLevel.ADMIN);

                                                    userApp.User.Roles.Remove(adminRole);
                                                }
                                            }
                                        }
                                    }
                                }

                                foreach (var userRole in user.Roles)
                                {
                                    userRole.Role.Users = new List<UserRole>();
                                }

                                foreach (var game in user.Games)
                                {
                                    game.User = null;
                                    game.SudokuMatrix.Difficulty.Matrices = new List<SudokuMatrix>();
                                }
                            }
                        }

                        var requestor = (User)(await usersRepository.GetById(requestorId)).Object;

                        if (!requestor.IsSuperUser)
                        {
                            // Filter out user emails from the frontend...
                            foreach (var user in result.Users)
                            {
                                user.Email = null;
                            }
                        }

                        result.Success = response.Success;
                        result.Message = UsersMessages.UsersFoundMessage;

                        return result;
                    }
                    else if (!response.Success && response.Exception != null)
                    {
                        result.Success = response.Success;
                        result.Message = response.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = UsersMessages.UsersNotFoundMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IAppResult> CreateApp(ILicenseRequest request)
        {
            var result = new AppResult();

            try
            {
                if (await usersRepository.IsUserRegistered(request.OwnerId))
                {
                    var generatingGuid = true;
                    var license = new Guid();

                    /* Ensure the license is unique by pulling all apps from the repository
                     * and checking that the new license is unique */
                    var checkAppsResponse = await appsRepository.GetAll();

                    do
                    {
                        license = Guid.NewGuid();

                        if (!checkAppsResponse.Objects.ConvertAll(a => (IApp)a).Any(a => a.License.Equals(license.ToString())))
                        {
                            generatingGuid = false;
                        }
                        else
                        {
                            generatingGuid = true;
                        }

                    } while (generatingGuid);

                    var app = new App(
                        request.Name,
                        license.ToString(),
                        request.OwnerId,
                        request.DevUrl,
                        request.LiveUrl);

                    var addAppResponse = await appsRepository.Add(app);

                    if (addAppResponse.Success)
                    {
                        var user = (User)
                            (await usersRepository.GetById(request.OwnerId))
                            .Object;

                        if (user.Roles.Any(ur => ur.Role.RoleLevel == RoleLevel.ADMIN))
                        {
                            var appAdmin = new AppAdmin(app.Id, user.Id);

                            _ = await appAdminsRepository.Add(appAdmin);
                        }

                        result.Success = addAppResponse.Success;
                        result.Message = AppsMessages.AppCreatedMessage;
                        result.App = (IApp)addAppResponse.Object;

                        return result;
                    }
                    else if (!addAppResponse.Success && addAppResponse.Exception != null)
                    {
                        result.Success = addAppResponse.Success;
                        result.Message = addAppResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = AppsMessages.AppNotCreatedMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = UsersMessages.UserDoesNotExistMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<ILicenseResult> GetLicense(int id)
        {
            var result = new LicenseResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    result.Success = true;
                    result.Message = AppsMessages.AppFoundMessage;
                    result.License = await appsRepository.GetLicense(id);

                    return result;
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IAppResult> UpdateApp(int id, IAppRequest request)
        {
            var result = new AppResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    var getAppResponse = await appsRepository.GetById(id);

                    if (getAppResponse.Success)
                    {
                        ((IApp)getAppResponse.Object).Name = request.Name;
                        ((IApp)getAppResponse.Object).DevUrl = request.DevUrl;
                        ((IApp)getAppResponse.Object).LiveUrl = request.LiveUrl;
                        ((IApp)getAppResponse.Object).IsActive = request.IsActive;
                        ((IApp)getAppResponse.Object).InDevelopment = request.InDevelopment;
                        ((IApp)getAppResponse.Object).PermitSuperUserAccess = request.PermitSuperUserAccess;
                        ((IApp)getAppResponse.Object).PermitCollectiveLogins = request.PermitCollectiveLogins;
                        ((IApp)getAppResponse.Object).DisableCustomUrls = request.DisableCustomUrls;
                        ((IApp)getAppResponse.Object).CustomEmailConfirmationDevUrl = request.CustomEmailConfirmationDevUrl;
                        ((IApp)getAppResponse.Object).CustomEmailConfirmationLiveUrl = request.CustomEmailConfirmationLiveUrl;
                        ((IApp)getAppResponse.Object).CustomPasswordResetDevUrl = request.CustomPasswordResetDevUrl;
                        ((IApp)getAppResponse.Object).CustomPasswordResetLiveUrl = request.CustomPasswordResetLiveUrl;
                        ((IApp)getAppResponse.Object).DateUpdated = DateTime.UtcNow;

                        var updateAppResponse = await appsRepository
                            .Update((App)getAppResponse.Object);

                        if (updateAppResponse.Success)
                        {
                            result.Success = true;
                            result.Message = AppsMessages.AppUpdatedMessage;
                            result.App = (App)updateAppResponse.Object;

                            return result;
                        }
                        else if (!updateAppResponse.Success && updateAppResponse.Exception != null)
                        {
                            result.Success = updateAppResponse.Success;
                            result.Message = updateAppResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = AppsMessages.AppNotUpdatedMessage;

                            return result;
                        }
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = getAppResponse.Exception.Message;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> AddAppUser(int userId, IBaseRequest request)
        {
            var result = new BaseResult();

            try
            {
                if (await appsRepository.IsAppLicenseValid(request.License))
                {
                    var addUserToAppResponse = await appsRepository.AddAppUser(
                        userId,
                        request.License);

                    if (addUserToAppResponse.Success)
                    {
                        result.Success = addUserToAppResponse.Success;
                        result.Message = AppsMessages.UserAddedToAppMessage;

                        return result;
                    }
                    else if (!addUserToAppResponse.Success && addUserToAppResponse.Exception != null)
                    {
                        result.Success = addUserToAppResponse.Success;
                        result.Message = addUserToAppResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = AppsMessages.UserNotAddedToAppMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> RemoveAppUser(int userId, IBaseRequest request)
        {
            var result = new BaseResult();

            try
            {
                if (await appsRepository.IsAppLicenseValid(request.License))
                {
                    var addUserToAppResponse = await appsRepository.RemoveAppUser(
                        userId,
                        request.License);

                    if (addUserToAppResponse.Success)
                    {
                        result.Success = addUserToAppResponse.Success;
                        result.Message = AppsMessages.UserRemovedFromAppMessage;

                        return result;
                    }
                    else if (!addUserToAppResponse.Success && addUserToAppResponse.Exception != null)
                    {
                        result.Success = addUserToAppResponse.Success;
                        result.Message = addUserToAppResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = AppsMessages.UserNotRemovedFromAppMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> DeleteOrResetApp(int id, bool isReset = false)
        {
            var result = new AppResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    var getAppResponse = await appsRepository.GetById(id, true);

                    if (isReset)
                    {
                        if (getAppResponse.Success)
                        {
                            var resetAppResponse = await appsRepository.Reset((App)getAppResponse.Object);

                            if (resetAppResponse.Success)
                            {
                                result.Success = resetAppResponse.Success;
                                result.Message = AppsMessages.AppResetMessage;

                                return result;
                            }
                            else if (!resetAppResponse.Success && resetAppResponse.Exception != null)
                            {
                                result.Success = resetAppResponse.Success;
                                result.Message = resetAppResponse.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = AppsMessages.AppNotFoundMessage;

                                return result;
                            }
                        }
                        else if (!getAppResponse.Success && getAppResponse.Exception != null)
                        {
                            result.Success = getAppResponse.Success;
                            result.Message = getAppResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = AppsMessages.AppNotFoundMessage;

                            return result;
                        }
                    }
                    else
                    {
                        if (getAppResponse.Success)
                        {
                            var deleteAppResponse = await appsRepository.Delete((App)getAppResponse.Object);

                            if (deleteAppResponse.Success)
                            {
                                result.Success = deleteAppResponse.Success;
                                result.Message = AppsMessages.AppDeletedMessage;

                                return result;
                            }
                            else if (!deleteAppResponse.Success && deleteAppResponse.Exception != null)
                            {
                                result.Success = deleteAppResponse.Success;
                                result.Message = deleteAppResponse.Exception.Message;

                                return result;
                            }
                            else
                            {
                                result.Success = false;
                                result.Message = AppsMessages.AppNotDeletedMessage;

                                return result;
                            }
                        }
                        else if (!getAppResponse.Success && getAppResponse.Exception != null)
                        {
                            result.Success = getAppResponse.Success;
                            result.Message = getAppResponse.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = AppsMessages.AppNotFoundMessage;

                            return result;
                        }
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> ActivateApp(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    var activateAppResponse = await appsRepository.Activate(id);

                    if (activateAppResponse.Success)
                    {
                        result.Success = activateAppResponse.Success;
                        result.Message = AppsMessages.AppActivatedMessage;

                        return result;
                    }
                    else if (!activateAppResponse.Success && activateAppResponse.Exception != null)
                    {
                        result.Success = activateAppResponse.Success;
                        result.Message = activateAppResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = AppsMessages.AppNotActivatedMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IBaseResult> DeactivateApp(int id)
        {
            var result = new BaseResult();

            try
            {
                if (await appsRepository.HasEntity(id))
                {
                    var activateAppResponse = await appsRepository.Deactivate(id);

                    if (activateAppResponse.Success)
                    {
                        result.Success = activateAppResponse.Success;
                        result.Message = AppsMessages.AppDeactivatedMessage;

                        return result;
                    }
                    else if (!activateAppResponse.Success && activateAppResponse.Exception != null)
                    {
                        result.Success = activateAppResponse.Success;
                        result.Message = activateAppResponse.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = false;
                        result.Message = AppsMessages.AppNotDeactivatedMessage;

                        return result;
                    }
                }
                else
                {
                    result.Success = false;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IUserResult> PromoteToAdmin(IBaseRequest request)
        {
            var result = new UserResult();

            try
            {
                var appResult = await appsRepository.GetByLicense(request.License);

                if (appResult.Success)
                {
                    var userResult = await usersRepository.GetById(request.RequestorId);

                    if (userResult.Success)
                    {
                        var app = (App)appResult.Object;
                        var user = (User)userResult.Object;

                        if (await appAdminsRepository.HasAdminRecord(app.Id, user.Id))
                        {
                            result.Success = appResult.Success;
                            result.Message = UsersMessages.UserIsAlreadyAnAdminMessage;

                            return result;
                        }

                        if (!user.IsAdmin)
                        {
                            var adminRole = (await rolesRepository.GetAll())
                                .Objects
                                .ConvertAll(r => (Role)r)
                                .FirstOrDefault(r => r.RoleLevel == RoleLevel.ADMIN);

                            user.Roles.Add(new UserRole {
                                UserId = user.Id,
                                User = user,
                                RoleId = adminRole.Id,
                                Role = adminRole}) ;

                            user = (User)(await usersRepository.Update(user)).Object;
                        }

                        var appAdmin = new AppAdmin(app.Id, user.Id);

                        var appAdminResult = await appAdminsRepository.Add(appAdmin);

                        if (appAdminResult.Success)
                        {
                            result.User = (User)
                                (await usersRepository.GetById(request.RequestorId))
                                .Object;
                            result.Success = appAdminResult.Success;
                            result.Message = UsersMessages.UserHasBeenPromotedToAdminMessage;

                            return result;
                        }
                        else if (!appAdminResult.Success && appAdminResult.Exception != null)
                        {
                            result.Success = appAdminResult.Success;
                            result.Message = appAdminResult.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = UsersMessages.UserHasNotBeenPromotedToAdminMessage;

                            return result;
                        }
                    }
                    else if (!userResult.Success && userResult.Exception != null)
                    {
                        result.Success = userResult.Success;
                        result.Message = userResult.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = userResult.Success;
                        result.Message = UsersMessages.NoUserIsUsingThisEmailMessage;

                        return result;
                    }
                }
                else if (!appResult.Success && appResult.Exception != null)
                {
                    result.Success = appResult.Success;
                    result.Message = appResult.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = appResult.Success;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IUserResult> ActivateAdminPrivileges(IBaseRequest request)
        {
            var result = new UserResult();

            try
            {
                var appResult = await appsRepository.GetByLicense(request.License);

                if (appResult.Success)
                {
                    var userResult = await usersRepository.GetById(request.RequestorId);

                    if (userResult.Success)
                    {
                        var app = (App)appResult.Object;
                        var user = (User)userResult.Object;

                        if (!user.IsAdmin)
                        {
                            result.Success = false;
                            result.Message = UsersMessages.UserDoesNotHaveAdminPrivilegesMessage;

                            return result;
                        }

                        if (!await appAdminsRepository.HasAdminRecord(app.Id, user.Id))
                        {
                            result.Success = appResult.Success;
                            result.Message = AppsMessages.UserIsNotAnAssignedAdminMessage;

                            return result;
                        }

                        var appAdmin = (AppAdmin)
                            (await appAdminsRepository.GetAdminRecord(app.Id, user.Id))
                            .Object;

                        appAdmin.IsActive = true;

                        var appAdminResult = await appAdminsRepository.Update(appAdmin);

                        if (appAdminResult.Success)
                        {
                            result.User = (User)
                                (await usersRepository.GetById(user.Id))
                                .Object;
                            result.Success = appAdminResult.Success;
                            result.Message = AppsMessages.AdminPrivilegesActivatedMessage;

                            return result;
                        }
                        else if (!appAdminResult.Success && appAdminResult.Exception != null)
                        {
                            result.Success = appAdminResult.Success;
                            result.Message = appAdminResult.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = AppsMessages.ActivationOfAdminPrivilegesFailedMessage;

                            return result;
                        }
                    }
                    else if (!userResult.Success && userResult.Exception != null)
                    {
                        result.Success = userResult.Success;
                        result.Message = userResult.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = userResult.Success;
                        result.Message = UsersMessages.NoUserIsUsingThisEmailMessage;

                        return result;
                    }
                }
                else if (!appResult.Success && appResult.Exception != null)
                {
                    result.Success = appResult.Success;
                    result.Message = appResult.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = appResult.Success;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<IUserResult> DeactivateAdminPrivileges(IBaseRequest request)
        {
            var result = new UserResult();

            try
            {
                var appResult = await appsRepository.GetByLicense(request.License);

                if (appResult.Success)
                {
                    var userResult = await usersRepository.GetById(request.RequestorId);

                    if (userResult.Success)
                    {
                        var app = (App)appResult.Object;
                        var user = (User)userResult.Object;

                        if (!user.IsAdmin)
                        {
                            result.Success = false;
                            result.Message = UsersMessages.UserDoesNotHaveAdminPrivilegesMessage;

                            return result;
                        }

                        if (!await appAdminsRepository.HasAdminRecord(app.Id, user.Id))
                        {
                            result.Success = appResult.Success;
                            result.Message = AppsMessages.UserIsNotAnAssignedAdminMessage;

                            return result;
                        }

                        var appAdmin = (AppAdmin)
                            (await appAdminsRepository.GetAdminRecord(app.Id, user.Id))
                            .Object;

                        appAdmin.IsActive = false;

                        var appAdminResult = await appAdminsRepository.Update(appAdmin);

                        if (appAdminResult.Success)
                        {
                            result.User = (User)
                                (await usersRepository.GetById(user.Id))
                                .Object;
                            result.Success = appAdminResult.Success;
                            result.Message = AppsMessages.AdminPrivilegesDeactivatedMessage;

                            return result;
                        }
                        else if (!appAdminResult.Success && appAdminResult.Exception != null)
                        {
                            result.Success = appAdminResult.Success;
                            result.Message = appAdminResult.Exception.Message;

                            return result;
                        }
                        else
                        {
                            result.Success = false;
                            result.Message = AppsMessages.DeactivationOfAdminPrivilegesFailedMessage;

                            return result;
                        }
                    }
                    else if (!userResult.Success && userResult.Exception != null)
                    {
                        result.Success = userResult.Success;
                        result.Message = userResult.Exception.Message;

                        return result;
                    }
                    else
                    {
                        result.Success = userResult.Success;
                        result.Message = UsersMessages.NoUserIsUsingThisEmailMessage;

                        return result;
                    }
                }
                else if (!appResult.Success && appResult.Exception != null)
                {
                    result.Success = appResult.Success;
                    result.Message = appResult.Exception.Message;

                    return result;
                }
                else
                {
                    result.Success = appResult.Success;
                    result.Message = AppsMessages.AppNotFoundMessage;

                    return result;
                }
            }
            catch (Exception exp)
            {
                result.Success = false;
                result.Message = exp.Message;

                return result;
            }
        }

        public async Task<bool> IsRequestValidOnThisLicense(int id, string license, int userId)
        {
            if (await usersRepository.HasEntity(userId))
            {
                if (await appsRepository.IsAppLicenseValid(license))
                {
                    var requestor = (User)(await usersRepository.GetById(userId, true)).Object;
                    var validLicense = await appsRepository.IsAppLicenseValid(license);
                    var requestorRegisteredToApp = await appsRepository.IsUserRegisteredToApp(id, license, userId);

                    if (requestorRegisteredToApp && validLicense)
                    {
                        var app = (App)(await appsRepository.GetByLicense(license)).Object;

                        if (app.IsActive)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else if (requestor.IsSuperUser && validLicense)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> IsOwnerOfThisLicense(int id, string license, int userId)
        {
            if (await usersRepository.HasEntity(userId))
            {
                if (await appsRepository.IsAppLicenseValid(license))
                {
                    var requestor = (User)(await usersRepository.GetById(userId, false)).Object;
                    var validLicense = await appsRepository.IsAppLicenseValid(license);
                    var requestorOwnerOfThisApp = await appsRepository.IsUserOwnerOfApp(id, license, userId);

                    if (requestorOwnerOfThisApp && validLicense)
                    {
                        return true;
                    }
                    else if (requestor.IsSuperUser && validLicense)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}
