using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
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
        public async Task<AppTaskResult> CreateApp(
            string name, int ownerId, string devUrl, string liveUrl) {

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

                var license = Guid.NewGuid();

                var app = new App(
                    name, 
                    license.ToString(), 
                    ownerId, 
                    devUrl, 
                    liveUrl);

                _context.Apps.Add(app);
                await _context.SaveChangesAsync();

                appTaskResult.Result = true;
                appTaskResult.App = app;

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

        public bool ValidLicense(string license) {

            var result = _context.Apps.Any(a => a.License.Equals(license));

            return result;
        }
    }
}
