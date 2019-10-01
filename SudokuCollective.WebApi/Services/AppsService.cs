using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;
using SudokuCollective.WebApi.Models.TaskModels.AppRequests;
using SudokuCollective.WebApi.Models.TaskModels.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class AppsService : IAppsService {

        private readonly ApplicationDbContext _context;

        public AppsService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<AppTaskResult> GetApp(int id, bool fullRecord = true) {

            var createdDate = DateTime.UtcNow;

            var appTaskResult = new AppTaskResult() {

                Result = false,
                App = new App() {

                    Id = 0,
                    Name = string.Empty,
                    License = "Unauthorized",
                    OwnerId = 0,
                    DateCreated = createdDate,
                    DateUpdated = createdDate,
                    DevUrl = string.Empty,
                    LiveUrl = string.Empty
                }
            };

            try {

                if (_context.Apps.Any(app => app.Id == id)) {

                    var app = new App();

                    if (fullRecord) {

                        app = await _context.Apps
                            .Include(a => a.Users)
                            .Where(a => a.Id == id)
                            .FirstOrDefaultAsync();

                        foreach (var user in app.Users) {

                            user.User = await _context.Users
                                .Include(u => u.Games)
                                .Include(u => u.Roles)
                                .Where(u => u.Id == user.UserId)
                                .FirstOrDefaultAsync();

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
                            .Where(a => a.Id == id)
                            .FirstOrDefaultAsync();

                        foreach (var user in app.Users) {

                            user.User = await _context.Users
                                .Where(u => u.Id == user.UserId)
                                .FirstOrDefaultAsync();
                        }
                    }

                    appTaskResult.Result = true;
                    appTaskResult.App = app;
                }

                return appTaskResult;

            } catch (Exception) {

                return appTaskResult;
            }
        }

        public async Task<AppListTaskResult> GetApps(bool fullRecord = true) {

            var appListTaskResult = new AppListTaskResult() {

                Result = false,
                Apps = new List<App>()
            };

            try {

                var apps = new List<App>();

                if (fullRecord) {

                    apps = await _context.Apps
                        .Include(a => a.Users)
                        .ToListAsync();

                    foreach (var app in apps) {

                        foreach (var ua in app.Users) {

                            ua.User = await _context.Users
                                .Include(u => u.Games)
                                .Include(u => u.Roles)
                                .Where(u => u.Id == ua.UserId)
                                .FirstOrDefaultAsync();

                            ua.User.Games = await _context.Games
                                .Include(g => g.SudokuMatrix)
                                .Where(g => g.User.Id == ua.UserId)
                                .ToListAsync();                    
                            
                            foreach (var game in ua.User.Games) {

                                game.SudokuMatrix = 
                                    await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                            }
                        }
                    }

                    appListTaskResult.Result = true;
                    appListTaskResult.Apps = apps;

                } else {

                    apps = await _context.Apps.ToListAsync();

                    appListTaskResult.Result = true;
                    appListTaskResult.Apps = apps;
                }

                return appListTaskResult;

            } catch (Exception) {

                return appListTaskResult;
            }
        }

        public async Task<AppTaskResult> CreateApp(LicenseRequestRO licenseRequestRO) {

            var createdDate = DateTime.UtcNow;

            var appTaskResult = new AppTaskResult() {

                Result = false,
                App = new App() {

                    Id = 0,
                    Name = string.Empty,
                    License = "Unauthorized",
                    OwnerId = 0,
                    DateCreated = createdDate,
                    DateUpdated = createdDate,
                    DevUrl = string.Empty,
                    LiveUrl = string.Empty,
                    IsActive = false
                }
            };

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
                        .Where(u => u.Id == licenseRequestRO.OwnerId)
                        .FirstOrDefaultAsync();

                    foreach (var userRole in owner.Roles) {

                        userRole.Role = await _context.Roles
                            .Where(r => r.Id == userRole.RoleId)
                            .FirstOrDefaultAsync();
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
                            .Where(r => r.RoleLevel == RoleLevel.ADMIN)
                            .FirstOrDefaultAsync();

                        var newUserAdminRole = new UserRole () {

                                UserId = owner.Id,
                                User = owner,
                                RoleId = adminRole.Id,
                                Role = adminRole
                            };

                        _context.UsersRoles.Add(newUserAdminRole);
                        await _context.SaveChangesAsync();
                    }
                    
                    appTaskResult.Result = true;                        
                    appTaskResult.App = app;
                }
                
                return appTaskResult;

            } catch (Exception) {

                return appTaskResult;
            }
        }
        
        public async Task<AppTaskResult> GetAppByLicense(string license, bool fullRecord = true) {

            var createdDate = DateTime.UtcNow;

            var appTaskResult = new AppTaskResult() {

                Result = false,
                App = new App() {

                    Id = 0,
                    Name = string.Empty,
                    License = "Unauthorized",
                    OwnerId = 0,
                    DateCreated = createdDate,
                    DateUpdated = createdDate,
                    DevUrl = string.Empty,
                    LiveUrl = string.Empty
                }
            };

            try {

                if (_context.Apps.Any(app => app.License.Equals(license))) {

                    var app = new App();

                    if (fullRecord) {

                        app = await _context.Apps
                            .Include(a => a.Users)
                            .Where(a => a.License.Equals(license))
                            .FirstOrDefaultAsync();

                        foreach (var user in app.Users) {

                            user.User = await _context.Users
                                .Include(u => u.Games)
                                .Include(u => u.Roles)
                                .Where(u => u.Id == user.UserId)
                                .FirstOrDefaultAsync();

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
                            .Where(a => a.License.Equals(license))
                            .FirstOrDefaultAsync();

                        foreach (var user in app.Users) {

                            user.User = await _context.Users
                                .Where(u => u.Id == user.UserId)
                                .FirstOrDefaultAsync();
                        }
                    }

                    appTaskResult.Result = true;
                    appTaskResult.App = app;
                }

                return appTaskResult;

            } catch (Exception) {

                return appTaskResult;
            }
        }

        public async Task<LicenseTaskResult> GetLicense(int id) {

            var licenseTaskResult = new LicenseTaskResult() {

                Result = false,
                License = string.Empty
            };

            try {

                if (_context.Apps.Any(a => a.Id == id)) {

                    var license = await _context.Apps
                        .Where(a => a.Id == id)
                        .Select(a => a.License)
                        .FirstOrDefaultAsync();
                    
                    licenseTaskResult.Result = true;
                    licenseTaskResult.License = license;
                }

                return licenseTaskResult;

            } catch (Exception) {

                return licenseTaskResult;
            }
        }
        
        public async Task<UserListTaskResult> GetAppUsers(
            BaseRequestRO baseRequest, bool fullRecord = true) {

            var users = new List<User>();

            var userListTaskResult = new UserListTaskResult() {

                Result = false,
                Users = users
            };

            try {

                var app = await _context.Apps
                    .Include(a => a.Users)
                    .Where(a => a.License.Equals(baseRequest.License))
                    .FirstOrDefaultAsync();

                if (fullRecord) {

                    foreach (var user in app.Users) {

                        user.User = await _context.Users
                            .Include(u => u.Games)
                            .Include(u => u.Roles)
                            .Where(u => u.Id == user.UserId)
                            .FirstOrDefaultAsync();

                        user.User.Roles = await _context.UsersRoles
                            .Include(ur => ur.Role)
                            .Where(ur => ur.UserId == user.UserId)
                            .ToListAsync();

                        foreach (var userRole in user.User.Roles) {

                            userRole.Role.Users = null;
                        }
                    }

                } else {

                    foreach (var user in app.Users) {

                        user.User = await _context.Users
                            .Include(u => u.Games)
                            .Where(u => u.Id == user.UserId)
                            .FirstOrDefaultAsync();
                    }
                }

                users = app.Users.Select(ua => ua.User).ToList();

                foreach (var user in users) {

                    user.Apps = null;
                }

                userListTaskResult.Result = true;
                userListTaskResult.Users = users;

                return userListTaskResult;

            } catch (Exception) {

                return userListTaskResult;
            }
        }

        public async Task<bool> UpdateApp(UpdateAppRO updateAppRO) {

            var result = false;
            
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

                return result = true;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> AddAppUser(int userId, BaseRequestRO baseRequestRO) {

            var result = false;

            try {

                var user = await _context.Users
                    .FirstOrDefaultAsync(
                        predicate: u => u.Id == userId);

                var app = await _context.Apps
                    .FirstOrDefaultAsync(
                        predicate: a => a.License.Equals(baseRequestRO.License));

                _context.UsersApps.Add(

                    new UserApp {

                        UserId = user.Id,
                        User = user,
                        AppId = app.Id,
                        App = app
                    }
                );

                await _context.SaveChangesAsync();
                return result = true;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> RemoveAppUser(int id, BaseRequestRO baseRequestRO) {

            var result = false;

            try {

                var userApp = await _context.UsersApps
                    .FirstOrDefaultAsync(
                        predicate: ua => ua.UserId == id);

                _context.UsersApps.Remove(userApp);

                await _context.SaveChangesAsync();
                return result = true;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> DeleteApp(int id) {

            var result = false;

            try {

                var app = await _context.Apps.FindAsync(id);

                if (app != null) {

                    _context.Apps.Remove(app);
                    await _context.SaveChangesAsync();

                    result = true;
                }

                return result;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> ActivateApp(int id) {

            var result = false;

            try {

                var app = await _context.Apps.FindAsync(id);

                if (app != null) {

                    if (!app.IsActive) {

                        app.ActivateApp();
                        _context.Apps.Update(app);
                        await _context.SaveChangesAsync();
                    }

                    result = true;
                }

                return result;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> DeactivateApp(int id) {

            var result = false;

            try {

                var app = await _context.Apps.FindAsync(id);

                if (app != null) {

                    if (app.IsActive) {
                        
                        app.DeactivateApp();
                        _context.Apps.Update(app);
                        await _context.SaveChangesAsync();
                    }

                    result = true;
                }

                return result;

            } catch (Exception) {

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
