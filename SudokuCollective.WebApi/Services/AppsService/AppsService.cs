using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Helpers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.AppRequests;
using SudokuCollective.WebApi.Models.ResultModels.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class AppsService : IAppsService {

        private readonly DatabaseContext _context;

        public AppsService(DatabaseContext context) {

            _context = context;
        }

        public async Task<AppResult> GetApp(int id, bool fullRecord = false) {

            var createdDate = DateTime.UtcNow;

            var appResult = new AppResult();

            try {

                if (_context.Apps.Any(app => app.Id == id)) {

                    var app = new App();

                    if (fullRecord) {

                        app = await _context.Apps
                            .Include(a => a.Users)
                            .FirstOrDefaultAsync(predicate: a => a.Id == id);

                        if (app == null) {

                            appResult.Message = "App not found";

                            return appResult;
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

                            appResult.Message = "App not found";

                            return appResult;
                        }

                        foreach (var user in app.Users) {

                            user.User = await _context.Users
                                .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);
                        }
                    }

                    appResult.Success = true;
                    appResult.App = app;

                } else {

                    appResult.Message = "App not found";
                }

                return appResult;

            } catch (Exception e) {

                appResult.Message = e.Message;

                return appResult;
            }
        }

        public async Task<AppsResult> GetApps(
            PageListModel pageListModel, 
            bool fullRecord = false) {

            var appListTaskResult = new AppsResult();

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

                                await game.SudokuMatrix.AttachSudokuCells(_context);
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

        public async Task<AppResult> CreateApp(LicenseRequest licenseRequest) {

            var appResult = new AppResult();

            try {

                if (_context.Users.Any(u => u.Id == licenseRequest.OwnerId)) {

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
                        .FirstOrDefaultAsync(predicate: u => u.Id == licenseRequest.OwnerId);

                    foreach (var userRole in owner.Roles) {

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
                    
                    appResult.Success = true;                        
                    appResult.App = app;

                } else {

                    appResult.Message = "Intended owner id does not exist";
                }

                return appResult;

            } catch (Exception e) {

                appResult.Message = e.Message;

                return appResult;
            }
        }
        
        public async Task<AppResult> GetAppByLicense(string license, bool fullRecord = false) {

            var appResult = new AppResult();

            try {

                if (_context.Apps.Any(app => app.License.Equals(license))) {

                    var app = new App();

                    if (fullRecord) {

                        app = await _context.Apps
                            .Include(a => a.Users)
                            .FirstOrDefaultAsync(predicate: a => a.License.Equals(license));

                        if (app == null) {

                            appResult.Message = "App not found";

                            return appResult;
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

                            appResult.Message = "App not found";

                            return appResult;
                        }

                        foreach (var user in app.Users) {

                            user.User = await _context.Users
                                .FirstOrDefaultAsync(predicate: u => u.Id == user.UserId);
                        }
                    }

                    appResult.Success = true;
                    appResult.App = app;

                } else {

                    appResult.Message = "App not found";
                }

                return appResult;

            } catch (Exception e) {

                appResult.Message = e.Message;

                return appResult;
            }
        }

        public async Task<LicenseResult> GetLicense(int id) {

            var licenseTaskResult = new LicenseResult();

            try {

                if (_context.Apps.Any(a => a.Id == id)) {

                    var license = await _context.Apps
                        .Where(a => a.Id == id)
                        .Select(a => a.License)
                        .FirstOrDefaultAsync();
                    
                    licenseTaskResult.Success = true;
                    licenseTaskResult.License = license;

                } else {

                    licenseTaskResult.Message = "App not found";
                }

                return licenseTaskResult;

            } catch (Exception e) {

                licenseTaskResult.Message = e.Message;

                return licenseTaskResult;
            }
        }
        
        public async Task<UsersResult> GetAppUsers(
            BaseRequest baseRequest, 
            bool fullRecord = false) {

            return await AppsServiceUtilities
                .RetrieveUsers(baseRequest, fullRecord, _context);
        }

        public async Task<BaseResult> UpdateApp(AppRequest appRequest) {

            var result = new BaseResult();
            
            try {

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

            } catch (Exception e) {

                result.Message = e.Message;

                return result;
            }
        }

        public async Task<BaseResult> AddAppUser(int userId, BaseRequest baseRequest) {

            var result = new BaseResult();

            try {

                var user = await _context.Users
                    .FirstOrDefaultAsync(predicate: u => u.Id == userId);

                var app = await _context.Apps
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(baseRequest.License));

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

        public async Task<BaseResult> RemoveAppUser(int id, BaseRequest baseRequest) {

            var result = new BaseResult();

            try {

                var app = await _context.Apps
                    .FirstOrDefaultAsync(predicate: a => a.License.Equals(baseRequest.License));

                var userApp = await _context.UsersApps
                    .FirstOrDefaultAsync(predicate: ua => ua.UserId == id && ua.AppId == app.Id);

                _context.UsersApps.Remove(userApp);

                await _context.SaveChangesAsync();
                result.Success = true;

                return result;

            } catch (Exception e) {

                result.Message = e.Message;

                return result;
            }
        }

        public async Task<BaseResult> DeleteApp(int id) {

            var result = new BaseResult();

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

        public async Task<BaseResult> ActivateApp(int id) {

            var result = new BaseResult();

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

        public async Task<BaseResult> DeactivateApp(int id) {

            var result = new BaseResult();

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
            var requestor = await _context.Users.FirstOrDefaultAsync(user => user.Id == userId);
            var validLicense = _context.Apps.Any(a => a.License.Equals(license));
            var requestorRegisteredToApp = _context.Apps.Where(a => a.License.Equals(license)).Any(a => a.Users.Any(ua => ua.UserId == userId));

            if (requestorRegisteredToApp && validLicense) {
                
                result = true;

            } else if (requestor.IsSuperUser && validLicense) {

                result = true;

            } else {

                result = false;
            }

            return result;
        }
    }
}
