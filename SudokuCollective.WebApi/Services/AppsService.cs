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
using SudokuCollective.WebApi.Models.TaskModels.AppRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class AppsService : IAppsService {

        private readonly ApplicationDbContext _context;

        public AppsService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<AppTaskResult> GetApp(int id, bool fullRecord = true) {

            var appTaskResult = new AppTaskResult() {

                Result = false,
                App = new App() {

                    Id = 0,
                    Name = string.Empty,
                    License = "Unauthorized",
                    OwnerId = 0,
                    DateCreated = DateTime.UtcNow,
                    DevUrl = string.Empty,
                    LiveUrl = string.Empty
                }
            };

            try {

                var app = new App();

                if (fullRecord) {

                    app = await _context.Apps
                        .Include(a => a.Users)
                        .SingleOrDefaultAsync(d => d.Id == id);

                    if (app == null) {

                        app = new App() {

                            Id = 0,
                            Name = string.Empty,
                            License = "Unauthorized",
                            OwnerId = 0,
                            DateCreated = DateTime.UtcNow,
                            DevUrl = string.Empty,
                            LiveUrl = string.Empty
                        };
                    }

                    foreach (var ua in app.Users) {

                        ua.User = await _context.Users
                            .Where(u => u.Id == ua.UserId)
                            .Include(u => u.Roles)
                            .FirstOrDefaultAsync();

                        ua.User.Games = await _context.Games
                            .Where(g => g.User.Id == ua.UserId)
                            .Include(g => g.SudokuMatrix)
                            .ToListAsync();                    
                        
                        foreach (var game in ua.User.Games) {

                            game.SudokuMatrix = 
                                await StaticApiHelpers.AttachSudokuMatrix(game, _context);
                        }
                    }

                    appTaskResult.Result = true;
                    appTaskResult.App = app;

                } else {

                    app = await _context.Apps
                        .SingleOrDefaultAsync(d => d.Id == id);

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
                                .Where(u => u.Id == ua.UserId)
                                .Include(u => u.Roles)
                                .FirstOrDefaultAsync();

                            ua.User.Games = await _context.Games
                                .Where(g => g.User.Id == ua.UserId)
                                .Include(g => g.SudokuMatrix)
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

            var appTaskResult = new AppTaskResult() {

                Result = false,
                App = new App() {

                    Id = 0,
                    Name = string.Empty,
                    License = "Unauthorized",
                    OwnerId = 0,
                    DateCreated = DateTime.UtcNow,
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

                    if (owner.Roles.Any(ur => ur.Role.RoleLevel != RoleLevel.ADMIN)) {
                        
                        var role = await _context.Roles
                            .Where(r => r.RoleLevel == RoleLevel.ADMIN)
                            .FirstOrDefaultAsync();

                        owner.Roles.Add(

                            new UserRole () {

                                UserId = owner.Id,
                                RoleId = role.Id
                            }
                        );

                        _context.UsersRoles.UpdateRange(owner.Roles);
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
        
        public async Task<AppTaskResult> GetAppByLicense(string license) {

            var appTaskResult = new AppTaskResult() {

                Result = false,
                App = new App() {

                    Id = 0,
                    Name = string.Empty,
                    License = "Unauthorized",
                    OwnerId = 0,
                    DateCreated = DateTime.UtcNow,
                    DevUrl = string.Empty,
                    LiveUrl = string.Empty
                }
            };

            try {

                if (_context.Apps.Any(app => app.License.Equals(license))) {

                    var app = await _context.Apps
                        .Where(a => a.License.Equals(license))
                        .FirstOrDefaultAsync();

                    appTaskResult.Result = true;
                    appTaskResult.App = app;
                }

                return appTaskResult;

            } catch (Exception) {

                return appTaskResult;
            }
        }

        public async Task<bool> UpdateApp(int id, App app) {

            var result = false;
            
            try {

                if (id == app.Id) {

                    _context.Entry(app).State = EntityState.Modified;
                    
                    await _context.SaveChangesAsync();

                    result = true;
                }

                return result;

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
