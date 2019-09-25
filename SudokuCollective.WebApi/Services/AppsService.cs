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

                            user.User.Apps = await _context.UsersApps
                                .ToListAsync();

                            foreach (var userRole in user.User.Roles) {

                                userRole.Role.Users = null;
                            }
                        }

                    } else {

                        app = await _context.Apps
                            .Include(a => a.Users)
                            .Where(a => app.Id == id)
                            .FirstOrDefaultAsync();

                        foreach (var user in app.Users) {

                            user.User = await _context.Users
                                .Where(u => u.Id == user.UserId)
                                .FirstOrDefaultAsync();

                            user.User.Roles = await _context.UsersRoles
                                .Where(ur => ur.UserId == user.UserId)
                                .ToListAsync();

                            user.User.Apps = await _context.UsersApps
                                .ToListAsync();
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
                    LiveUrl = string.Empty
                }
            };

            try {

                if (_context.Users.Any(u => u.Id == licenseRequestRO.OwnerId)) {

                    var license = Guid.NewGuid();

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

        public async Task<bool> UpdateApp(UpdateAppRO updateAppRO) {

            var result = false;
            
            try {

                var app = await _context.Apps
                    .Include(a => a.Users)
                    .Where(a => a.License.Equals(updateAppRO.License))
                    .FirstOrDefaultAsync();
                
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

        public bool ValidLicense(string license) {

            var result = _context.Apps.Any(a => a.License.Equals(license));

            return result;
        }
    }
}
