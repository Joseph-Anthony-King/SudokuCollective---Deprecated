using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Helpers;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.Data.Services
{
    public class AppsService : IAppsService
    {

        private readonly DatabaseContext _context;

        public AppsService(DatabaseContext context)
        {

            _context = context;
        }

        public async Task<IAppResult> GetApp(int id, bool fullRecord = false)
        {

            var createdDate = DateTime.UtcNow;

            var appResult = new AppResult();

            try
            {

                if (_context.Apps.Any(app => app.Id == id))
                {

                    var app = new App();

                    if (fullRecord)
                    {

                        app = (App)await _context.Apps
                            .Include(a => a.Users)
                            .FirstOrDefaultAsync(predicate: a => a.Id == id);

                        if (app == null)
                        {

                            appResult.Message = "App not found";

                            return appResult;
                        }

                        foreach (var user in app.Users)
                        {

                            user.User = await _context.Users
                                .Include(u => u.Games)
                                .Include(u => u.Roles)
                                .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);

                            var query = await _context.UsersRoles
                                .Include(ur => ur.Role)
                                .Where(ur => ur.UserId == user.UserId)
                                .ToListAsync();

                            user.User.Roles = query.ConvertAll(ur => ur as IUserRole);

                            foreach (var userRole in user.User.Roles)
                            {

                                userRole.Role.Users = null;
                            }
                        }

                    }
                    else
                    {

                        app = (App)await _context.Apps
                            .Include(a => a.Users)
                            .FirstOrDefaultAsync(predicate: a => a.Id == id);

                        if (app == null)
                        {

                            appResult.Message = "App not found";

                            return appResult;
                        }

                        foreach (var user in app.Users)
                        {

                            user.User = await _context.Users
                                .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);
                        }
                    }

                    appResult.Success = true;
                    appResult.App = app;

                }
                else
                {

                    appResult.Message = "App not found";
                }

                return appResult;

            }
            catch (Exception e)
            {

                appResult.Message = e.Message;

                return appResult;
            }
        }

        public async Task<IAppsResult> GetApps(
            IPageListModel pageListModel,
            bool fullRecord = false)
        {

            var appListTaskResult = new AppsResult();

            try
            {

                var apps = new List<App>();

                if (fullRecord)
                {

                    apps = await AppsServiceUtilities
                        .RetrieveApps(pageListModel, _context);

                    foreach (var app in apps)
                    {

                        foreach (var ua in app.Users)
                        {

                            ua.User = await _context.Users
                                .Include(u => u.Games)
                                .Include(u => u.Roles)
                                .FirstOrDefaultAsync(predicate: u => u.Id == ua.UserId);

                            var query = await _context.Games
                                .OrderBy(g => g.Id)
                                .Include(g => g.SudokuMatrix)
                                .Where(g => g.User.Id == ua.UserId)
                                .ToListAsync();

                            ua.User.Games = query.ConvertAll(g => g as IGame);

                            foreach (var game in ua.User.Games)
                            {

                                await game.SudokuMatrix.AttachSudokuCells(_context);
                            }
                        }
                    }

                    appListTaskResult.Success = true;
                    appListTaskResult.Apps = apps.ConvertAll(a => (IApp)a);

                }
                else
                {

                    apps = await AppsServiceUtilities
                        .RetrieveApps(pageListModel, _context);

                    appListTaskResult.Success = true;
                    appListTaskResult.Apps = apps.ConvertAll(a => (IApp)a);
                }

                return appListTaskResult;

            }
            catch (Exception e)
            {

                appListTaskResult.Message = e.Message;

                return appListTaskResult;
            }
        }

        public async Task<IAppResult> CreateApp(ILicenseRequest licenseRequest)
        {

            var appResult = new AppResult();

            try
            {

                if (_context.Users.Any(u => u.Id == licenseRequest.OwnerId))
                {

                    var generatingGuid = true;
                    var license = new Guid();

                    var apps = await _context.Apps.ToListAsync();

                    do
                    {

                        license = Guid.NewGuid();

                        if (!apps.Any(a => a.License.Equals(license.ToString())))
                        {

                            generatingGuid = false;
                        }

                    } while (generatingGuid);

                    var owner = await _context.Users
                        .Include(u => u.Roles)
                        .FirstOrDefaultAsync(predicate: u => u.Id == licenseRequest.OwnerId);

                    foreach (var userRole in owner.Roles)
                    {

                        userRole.Role = await _context.Roles
                            .FirstOrDefaultAsync(predicate: r => r.Id == userRole.RoleId);
                    }

                    var app = new App(
                        licenseRequest.Name,
                        license.ToString(),
                        licenseRequest.OwnerId,
                        licenseRequest.DevUrl,
                        licenseRequest.LiveUrl);

                    _context.Apps.Add(app);
                    await _context.SaveChangesAsync();

                    var userApp = new UserApp()
                    {

                        UserId = owner.Id,
                        AppId = app.Id
                    };

                    _context.UsersApps.Add(userApp);
                    await _context.SaveChangesAsync();

                    var addAdminRole = true;

                    foreach (var userRole in owner.Roles)
                    {

                        if (userRole.Role.RoleLevel == RoleLevel.ADMIN)
                        {

                            addAdminRole = false;
                        }
                    }

                    if (addAdminRole)
                    {

                        var adminRole = await _context.Roles
                            .FirstOrDefaultAsync(predicate: r => r.RoleLevel == RoleLevel.ADMIN);

                        var newUserAdminRole = new UserRole()
                        {

                            UserId = owner.Id,
                            User = owner,
                            RoleId = adminRole.Id,
                            Role = adminRole
                        };

                        _context.UsersRoles.Add(newUserAdminRole);
                        await _context.SaveChangesAsync();
                    }

                    appResult.Success = true;
                    appResult.App = app;

                }
                else
                {
                    appResult.Message = "Intended owner id does not exist";
                }

                return appResult;

            }
            catch (Exception e)
            {
                appResult.Message = e.Message;

                return appResult;
            }
        }

        public async Task<IAppResult> GetAppByLicense(string license, bool fullRecord = false)
        {
            var appResult = new AppResult();

            try
            {
                if (_context.Apps.Any(app => app.License.Equals(license)))
                {
                    var app = new App();

                    if (fullRecord)
                    {
                        app = (App)await _context.Apps
                            .Include(a => a.Users)
                            .FirstOrDefaultAsync(predicate: a => a.License.Equals(license));

                        if (app == null)
                        {
                            appResult.Message = "App not found";

                            return appResult;
                        }

                        foreach (var user in app.Users)
                        {
                            user.User = await _context.Users
                                .Include(u => u.Games)
                                .Include(u => u.Roles)
                                .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);

                            var query = await _context.UsersRoles
                                .Include(ur => ur.Role)
                                .Where(ur => ur.UserId == user.UserId)
                                .ToListAsync();

                            user.User.Roles = query.ConvertAll(ur => ur as IUserRole);

                            foreach (var userRole in user.User.Roles)
                            {
                                userRole.Role.Users = null;
                            }
                        }

                    }
                    else
                    {

                        app = (App)await _context.Apps
                            .Include(a => a.Users)
                            .FirstOrDefaultAsync(predicate: a => a.License.Equals(license));

                        if (app == null)
                        {
                            appResult.Message = "App not found";

                            return appResult;
                        }

                        foreach (var user in app.Users)
                        {
                            user.User = await _context.Users
                                .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);
                        }
                    }

                    appResult.Success = true;
                    appResult.App = app;
                }
                else
                {
                    appResult.Message = "App not found";
                }

                return appResult;

            }
            catch (Exception e)
            {
                appResult.Message = e.Message;

                return appResult;
            }
        }

        public async Task<ILicenseResult> GetLicense(int id)
        {
            var licenseTaskResult = new LicenseResult();

            try
            {
                if (_context.Apps.Any(a => a.Id == id))
                {
                    var license = await _context.Apps
                        .Where(a => a.Id == id)
                        .Select(a => a.License)
                        .FirstOrDefaultAsync();

                    licenseTaskResult.Success = true;
                    licenseTaskResult.License = license;
                }
                else
                {
                    licenseTaskResult.Message = "App not found";
                }

                return licenseTaskResult;
            }
            catch (Exception e)
            {
                licenseTaskResult.Message = e.Message;

                return licenseTaskResult;
            }
        }

        public async Task<IUsersResult> GetAppUsers(
            IBaseRequest baseRequest,
            bool fullRecord = false)
        {
            return await AppsServiceUtilities
                .RetrieveUsers(baseRequest, fullRecord, _context);
        }

        public async Task<IBaseResult> UpdateApp(IAppRequest appRequest)
        {
            var result = new BaseResult();

            try
            {
                var app = await _context.Apps
                    .Include(a => a.Users)
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(appRequest.License));

                app.Name = appRequest.Name;
                app.DevUrl = appRequest.DevUrl;
                app.LiveUrl = appRequest.LiveUrl;
                app.DateUpdated = DateTime.UtcNow;

                _context.Entry(app).State = EntityState.Modified;

                await _context.SaveChangesAsync();
                result.Success = true;

                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IBaseResult> AddAppUser(int userId, IBaseRequest baseRequest)
        {
            var result = new BaseResult();

            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(predicate: u => u.Id == userId);

                var app = await _context.Apps
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(baseRequest.License));

                _context.UsersApps.Add(

                    new UserApp
                    {
                        UserId = user.Id,
                        User = user,
                        AppId = app.Id,
                        App = app
                    }
                );

                await _context.SaveChangesAsync();
                result.Success = true;

                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IBaseResult> RemoveAppUser(int id, IBaseRequest baseRequest)
        {
            var result = new BaseResult();

            try
            {
                var app = await _context.Apps
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(baseRequest.License));

                var userApp = await _context.UsersApps
                    .FirstOrDefaultAsync(predicate: ua => ua.UserId == id && ua.AppId == app.Id);

                _context.UsersApps.Remove(userApp);

                await _context.SaveChangesAsync();
                result.Success = true;

                return result;
            }
            catch (Exception e)
            {
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IBaseResult> DeleteOrResetApp(int id, bool isReset = false)
        {
            var result = new BaseResult();

            try
            {
                var app = await _context.Apps.FindAsync(id);

                if (app != null)
                {
                    var users = await _context.Users
                        .Where(u => u.Apps.Any(ua => ua.AppId == app.Id))
                        .ToListAsync();

                    var games = await _context.Games
                        .Where(g => g.AppId == app.Id)
                        .ToListAsync();

                    var sudokuMatrixIds = new List<int>();
                    var solutionItems = new List<SolutionItem>();

                    foreach (var game in games)
                    {
                        sudokuMatrixIds.Add(game.SudokuMatrixId);
                        solutionItems.Add(new SolutionItem()
                        {
                            SolutionId = game.SudokuSolutionId,
                            ContinueGame = game.ContinueGame
                        });
                    }

                    var cells = new List<ISudokuCell>();
                    var matrices = new List<ISudokuMatrix>();
                    var solutions = new List<ISudokuSolution>();

                    foreach (var sudokuMatrixId in sudokuMatrixIds)
                    {
                        cells.AddRange(
                            await _context.SudokuCells
                            .Where(c => c.SudokuMatrixId == sudokuMatrixId)
                            .ToListAsync());

                        matrices.AddRange(
                            await _context.SudokuMatrices
                            .Where(m => m.Id == sudokuMatrixId)
                            .ToListAsync());
                    }

                    foreach (var solutionItem in solutionItems)
                    {

                        if (solutionItem.ContinueGame)
                        {

                            solutions.AddRange(
                                await _context.SudokuSolutions
                                .Where(s => s.Id == solutionItem.SolutionId)
                                .ToListAsync());
                        }
                    }

                    _context.SudokuCells.RemoveRange(cells.ConvertAll(c => c as SudokuCell));
                    _context.SudokuMatrices.RemoveRange(matrices.ConvertAll(m => m as SudokuMatrix));
                    _context.SudokuSolutions.RemoveRange(solutions.ConvertAll(s => s as SudokuSolution));
                    _context.Games.RemoveRange(games);

                    foreach (var user in users)
                    {
                        var u = (User)user;

                        if (u.Apps.Count == 1 && u.IsAdmin == false)
                        {
                            _context.Users.Remove(user);
                        }
                    }

                    if (!isReset)
                    {
                        _context.Apps.Remove(app);
                    }

                    await _context.SaveChangesAsync();

                    result.Success = true;
                    result.Message = string.Format("{0} app successfully deleted", app.Name);
                }

                return result;

            }
            catch (Exception e)
            {
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IBaseResult> ActivateApp(int id)
        {

            var result = new BaseResult();

            try
            {

                var app = await _context.Apps.FindAsync(id);

                if (app != null)
                {
                    if (!app.IsActive)
                    {
                        app.ActivateApp();
                        _context.Apps.Update(app);
                        await _context.SaveChangesAsync();
                    }

                    result.Success = true;
                }

                return result;

            }
            catch (Exception e)
            {

                result.Message = e.Message;

                return result;
            }
        }

        public async Task<IBaseResult> DeactivateApp(int id)
        {
            var result = new BaseResult();

            try
            {
                var app = await _context.Apps.FindAsync(id);

                if (app != null)
                {
                    if (app.IsActive)
                    {
                        app.DeactivateApp();
                        _context.Apps.Update(app);
                        await _context.SaveChangesAsync();
                    }

                    result.Success = true;
                }

                return result;

            }
            catch (Exception e)
            {
                result.Message = e.Message;

                return result;
            }
        }

        public async Task<bool> IsRequestValidOnThisLicense(string license, int userId, int appId)
        {
            var result = false;
            var requestor = (User)await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            var validLicense = _context.Apps.Any(a => a.License.Equals(license));
            var requestorRegisteredToApp = _context.Apps
                .Where(a => a.License.Equals(license))
                    .Any(a => a.Users.Any(ua => ua.UserId == userId) && a.Id == appId);

            if (requestorRegisteredToApp && validLicense)
            {
                result = true;
            }
            else if (requestor.IsSuperUser && validLicense)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }

        public async Task<bool> IsOwnerOfThisLicense(string license, int userId, int appId)
        {
            var result = false;
            var requestor = (User)await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            var validLicense = _context.Apps.Any(a => a.License.Equals(license));
            var requestorOwnerOfThisApp = _context.Apps
                .Any(predicate:
                    a => a.License.Equals(license)
                    && a.OwnerId == userId
                    && a.Id == appId);

            if (requestorOwnerOfThisApp && validLicense)
            {
                result = true;
            }
            else if (requestor.IsSuperUser && validLicense)
            {
                result = true;
            }
            else
            {
                result = false;
            }

            return result;
        }
    }
}
