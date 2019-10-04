using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;
using SudokuCollective.WebApi.Models.TaskModels;
using SudokuCollective.WebApi.Models.TaskModels.AppRequests;
using SudokuCollective.WebApi.Models.TaskModels.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class AppsService : IAppsService {

        private readonly ApplicationDbContext _context;

        public AppsService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<AppTaskResult> GetApp(int id, bool fullRecord = false) {

            var createdDate = DateTime.UtcNow;

            var appTaskResult = new AppTaskResult();

            try {

                if (_context.Apps.Any(app => app.Id == id)) {

                    var app = new App();

                    if (fullRecord) {

                        app = await _context.Apps
                            .Include(a => a.Users)
                            .FirstOrDefaultAsync(predicate: a => a.Id == id);

                        if (app == null) {

                            appTaskResult.Message = "App not found";

                            return appTaskResult;
                        }

                        foreach (var user in app.Users) {

                            user.User = await _context.Users
                                .Include(u => u.Games)
                                .Include(u => u.Roles)
                                .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);

                            user.User.Roles = await _context.UsersRoles
                                .Include(ur => ur.Role)
                                .Where(ur => ur.UserId == user.UserId)
                                .ToListAsync();

                            foreach (var userRole in user.User.Roles) {

                                userRole.Role.Users = null;
                            }
                        }

                    } else {

                        app = await _context.Apps
                            .Include(a => a.Users)
                            .FirstOrDefaultAsync(predicate: a => a.Id == id);

                        if (app == null) {

                            appTaskResult.Message = "App not found";

                            return appTaskResult;
                        }

                        foreach (var user in app.Users) {

                            user.User = await _context.Users
                                .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);
                        }
                    }

                    appTaskResult.Success = true;
                    appTaskResult.App = app;
                }

                return appTaskResult;

            } catch (Exception e) {

                appTaskResult.Message = e.Message;

                return appTaskResult;
            }
        }

        public async Task<AppListTaskResult> GetApps(
            PageListModel pageListModel, 
            bool fullRecord = false) {

            var appListTaskResult = new AppListTaskResult();

            try {

                var apps = new List<App>();

                if (fullRecord) {

                    apps = await AppsServiceUtilities
                        .RetrieveApps(pageListModel, _context);

                    foreach (var app in apps) {

                        foreach (var ua in app.Users) {

                            ua.User = await _context.Users
                                .Include(u => u.Games)
                                .Include(u => u.Roles)
                                .FirstOrDefaultAsync(predicate: u => u.Id == ua.UserId);

                            ua.User.Games = await _context.Games
                                .OrderBy(g => g.Id)
                                .Include(g => g.SudokuMatrix)
                                .Where(g => g.User.Id == ua.UserId)
                                .ToListAsync();                    
                            
                            foreach (var game in ua.User.Games) {

                                game.SudokuMatrix = 
                                    await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                            }
                        }
                    }

                    appListTaskResult.Success = true;
                    appListTaskResult.Apps = apps;

                } else {

                    apps = await AppsServiceUtilities
                        .RetrieveApps(pageListModel, _context);

                    appListTaskResult.Success = true;
                    appListTaskResult.Apps = apps;
                }

                return appListTaskResult;

            } catch (Exception e) {

                appListTaskResult.Message = e.Message;

                return appListTaskResult;
            }
        }

        public async Task<AppTaskResult> CreateApp(LicenseRequestRO licenseRequestRO) {

            var appTaskResult = new AppTaskResult();

            try {

                if (_context.Users.Any(u => u.Id == licenseRequestRO.OwnerId)) {

                    var generatingGuid = true;
                    var license = new Guid();

                    var apps = await _context.Apps.ToListAsync();

                    do {

                        license = Guid.NewGuid();

                        if (!apps.Any(a => a.License.Equals(license.ToString()))) {

                            generatingGuid = false;
                        }

                    } while (generatingGuid);

                    var owner = await _context.Users
                        .Include(u => u.Roles)
                        .FirstOrDefaultAsync(predicate: u => u.Id == licenseRequestRO.OwnerId);

                    foreach (var userRole in owner.Roles) {

                        userRole.Role = await _context.Roles
                            .FirstOrDefaultAsync(predicate: r => r.Id == userRole.RoleId);
                    }

                    var app = new App(
                        licenseRequestRO.Name, 
                        license.ToString(), 
                        licenseRequestRO.OwnerId, 
                        licenseRequestRO.DevUrl, 
                        licenseRequestRO.LiveUrl);

                    _context.Apps.Add(app);
                    await _context.SaveChangesAsync();

                    var userApp = new UserApp() {

                        UserId = owner.Id,
                        AppId = app.Id
                    };

                    _context.UsersApps.Add(userApp);
                    await _context.SaveChangesAsync();

                    var addAdminRole = true;

                    foreach (var userRole in owner.Roles) {
                        
                        if (userRole.Role.RoleLevel == RoleLevel.ADMIN) {

                            addAdminRole = false;
                        }
                    }

                    if (addAdminRole) {
                        
                        var adminRole = await _context.Roles
                            .FirstOrDefaultAsync(predicate: r => r.RoleLevel == RoleLevel.ADMIN);

                        var newUserAdminRole = new UserRole () {

                                UserId = owner.Id,
                                User = owner,
                                RoleId = adminRole.Id,
                                Role = adminRole
                            };

                        _context.UsersRoles.Add(newUserAdminRole);
                        await _context.SaveChangesAsync();
                    }
                    
                    appTaskResult.Success = true;                        
                    appTaskResult.App = app;
                }
                
                return appTaskResult;

            } catch (Exception e) {

                appTaskResult.Message = e.Message;

                return appTaskResult;
            }
        }
        
        public async Task<AppTaskResult> GetAppByLicense(string license, bool fullRecord = false) {

            var appTaskResult = new AppTaskResult();

            try {

                if (_context.Apps.Any(app => app.License.Equals(license))) {

                    var app = new App();

                    if (fullRecord) {

                        app = await _context.Apps
                            .Include(a => a.Users)
                            .FirstOrDefaultAsync(predicate: a => a.License.Equals(license));

                        if (app == null) {

                            appTaskResult.Message = "App not found";

                            return appTaskResult;
                        }

                        foreach (var user in app.Users) {

                            user.User = await _context.Users
                                .Include(u => u.Games)
                                .Include(u => u.Roles)
                                .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);

                            user.User.Roles = await _context.UsersRoles
                                .Include(ur => ur.Role)
                                .Where(ur => ur.UserId == user.UserId)
                                .ToListAsync();

                            foreach (var userRole in user.User.Roles) {

                                userRole.Role.Users = null;
                            }
                        }

                    } else {

                        app = await _context.Apps
                            .Include(a => a.Users)
                            .FirstOrDefaultAsync(predicate: a => a.License.Equals(license));

                        if (app == null) {

                            appTaskResult.Message = "App not found";

                            return appTaskResult;
                        }

                        foreach (var user in app.Users) {

                            user.User = await _context.Users
                                .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);
                        }
                    }

                    appTaskResult.Success = true;
                    appTaskResult.App = app;
                }

                return appTaskResult;

            } catch (Exception e) {

                appTaskResult.Message = e.Message;

                return appTaskResult;
            }
        }

        public async Task<LicenseTaskResult> GetLicense(int id) {

            var licenseTaskResult = new LicenseTaskResult();

            try {

                if (_context.Apps.Any(a => a.Id == id)) {

                    var license = await _context.Apps
                        .Where(a => a.Id == id)
                        .Select(a => a.License)
                        .FirstOrDefaultAsync();
                    
                    licenseTaskResult.Success = true;
                    licenseTaskResult.License = license;
                }

                return licenseTaskResult;

            } catch (Exception e) {

                licenseTaskResult.Message = e.Message;

                return licenseTaskResult;
            }
        }
        
        public async Task<UserListTaskResult> GetAppUsers(
            BaseRequestRO baseRequestRO, 
            bool fullRecord = false) {

            return await AppsServiceUtilities
                .RetrieveUsers(baseRequestRO, fullRecord, _context);
        }

        public async Task<BaseTaskResult> UpdateApp(UpdateAppRO updateAppRO) {

            var result = new BaseTaskResult();
            
            try {

                var app = await _context.Apps
                    .Include(a => a.Users)
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(updateAppRO.License));
                
                app.Name = updateAppRO.Name;
                app.DevUrl = updateAppRO.DevUrl;
                app.LiveUrl = updateAppRO.LiveUrl;
                app.DateUpdated = DateTime.UtcNow;

                _context.Entry(app).State = EntityState.Modified;
                
                await _context.SaveChangesAsync();
                result.Success = true;

                return result;

            } catch (Exception e) {

                result.Message = e.Message;

                return result;
            }
        }

        public async Task<BaseTaskResult> AddAppUser(int userId, BaseRequestRO baseRequestRO) {

            var result = new BaseTaskResult();

            try {

                var user = await _context.Users
                    .FirstOrDefaultAsync(predicate: u => u.Id == userId);

                var app = await _context.Apps
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(baseRequestRO.License));

                _context.UsersApps.Add(

                    new UserApp {

                        UserId = user.Id,
                        User = user,
                        AppId = app.Id,
                        App = app
                    }
                );

                await _context.SaveChangesAsync();
                result.Success = true;

                return result;

            } catch (Exception e) {

                result.Message = e.Message;

                return result;
            }
        }

        public async Task<BaseTaskResult> RemoveAppUser(int id, BaseRequestRO baseRequestRO) {

            var result = new BaseTaskResult();

            try {

                var userApp = await _context.UsersApps
                    .FirstOrDefaultAsync(predicate: ua => ua.UserId == id);

                _context.UsersApps.Remove(userApp);

                await _context.SaveChangesAsync();
                result.Success = true;

                return result;

            } catch (Exception e) {

                result.Message = e.Message;

                return result;
            }
        }

        public async Task<BaseTaskResult> DeleteApp(int id) {

            var result = new BaseTaskResult();

            try {

                var app = await _context.Apps.FindAsync(id);

                if (app != null) {

                    _context.Apps.Remove(app);
                    await _context.SaveChangesAsync();

                    result.Success = true;
                }

                return result;

            } catch (Exception e) {

                result.Message = e.Message;

                return result;
            }
        }

        public async Task<BaseTaskResult> ActivateApp(int id) {

            var result = new BaseTaskResult();

            try {

                var app = await _context.Apps.FindAsync(id);

                if (app != null) {

                    if (!app.IsActive) {

                        app.ActivateApp();
                        _context.Apps.Update(app);
                        await _context.SaveChangesAsync();
                    }

                    result.Success = true;
                }

                return result;

            } catch (Exception e) {

                result.Message = e.Message;

                return result;
            }
        }

        public async Task<BaseTaskResult> DeactivateApp(int id) {

            var result = new BaseTaskResult();

            try {

                var app = await _context.Apps.FindAsync(id);

                if (app != null) {

                    if (app.IsActive) {
                        
                        app.DeactivateApp();
                        _context.Apps.Update(app);
                        await _context.SaveChangesAsync();
                    }

                    result.Success = true;
                }

                return result;

            } catch (Exception e) {

                result.Message = e.Message;

                return result;
            }
        }

        public async Task<bool> IsRequestValidOnThisLicense(string license, int userId) {

            var result = false;
            var validLicense = _context.Apps.Any(a => a.License.Equals(license));

            /* The superuser has access system wide to all apps.  The if statement
               checks if the requestor is not the superuser, if they aren't it then
               checks if the requestor is a registered user for this app.  The else 
               if statement checks if the requestor is the superuser and if the 
               request is submitted with a valid license.  The final else statement 
               denies access if the license is invalid. */
            if (userId != 1 && validLicense) {

                var app = await _context.Apps
                    .Include(a => a.Users)
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(license));

                foreach (var userApp in app.Users) {

                    userApp.User = await _context.Users
                        .FirstOrDefaultAsync(predicate: u => u.Id == userApp.UserId);
                }

                if (app.Users.Any(user => user.User.Id == userId)) {

                    result = true;
                }

            } else if (userId == 1 && validLicense) {

                result = true;

            } else {

                result = false;
            }

            return result;
        }
    }
}
