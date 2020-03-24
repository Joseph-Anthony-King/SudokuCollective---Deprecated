using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.AppRequests;
using SudokuCollective.WebApi.Models.ResultModels.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.MockServices {

    public class MockAppsService {

        private DatabaseContext _context;
        internal Mock<IAppsService> AppsServiceSuccessfulRequest { get; set; }
        internal Mock<IAppsService> AppsServiceFailedRequest { get; set; }
        internal Mock<IAppsService> AppsServiceInvalidRequest { get; set; }

        public MockAppsService(DatabaseContext context) {

            _context = context;
            AppsServiceSuccessfulRequest = new Mock<IAppsService>();
            AppsServiceFailedRequest = new Mock<IAppsService>();
            AppsServiceInvalidRequest = new Mock<IAppsService>();

            AppsServiceSuccessfulRequest.Setup(appService => 
                appService.GetApp(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppResult() {
            
                        Success = true,
                        Message = string.Empty,
                        App = _context.Apps.FirstOrDefault(predicate: app => app.Id == 1)
                    }));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetAppByLicense(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppResult() {

                    Success = true,
                    Message = string.Empty,
                    App = _context.Apps.FirstOrDefault(predicate: app => app.License.Equals(TestObjects.GetLicense()))
                }));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetApps(It.IsAny<PageListModel>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppsResult() {

                    Success = true,
                    Message = string.Empty,
                    Apps = _context.Apps.ToList()
                }));

            AppsServiceSuccessfulRequest.Setup(appService => 
                appService.IsRequestValidOnThisLicense(
                    It.IsAny<string>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.IsOwnerOfThisLicense(
                    It.IsAny<string>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            AppsServiceSuccessfulRequest.Setup(appService => 
                appService.UpdateApp(It.IsAny<AppRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            AppsServiceSuccessfulRequest.Setup(appService => 
                appService.GetAppUsers(It.IsAny<BaseRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult() {

                    Success = true,
                    Message = string.Empty,
                    Users = context.Users
                        .Where(user => user.Apps.Any(userApp => userApp.AppId == 1))
                        .ToList()
                }));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetLicense(It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseResult() {

                    License = TestObjects.GetLicense(),
                    Success = true,
                    Message = string.Empty
                }));

            AppsServiceSuccessfulRequest.Setup(appService => 
                appService.AddAppUser(It.IsAny<int>(), It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            AppsServiceSuccessfulRequest.Setup(appService => 
                appService.RemoveAppUser(It.IsAny<int>(), It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            AppsServiceSuccessfulRequest.Setup(appService => 
                appService.ActivateApp(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            AppsServiceSuccessfulRequest.Setup(appService => 
                appService.DeactivateApp(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.DeleteApp(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            AppsServiceFailedRequest.Setup(appService => 
                appService.GetApp(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppResult() {
            
                        Success = false,
                        Message = "Error retrieving app",
                        App = new App()
                    }));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetAppByLicense(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppResult() {

                    Success = false,
                    Message = "Error retrieving app",
                    App = new App()
                }));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetApps(It.IsAny<PageListModel>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppsResult() {

                    Success = false,
                    Message = "Error retrieving apps",
                    Apps = new List<App>()
                }));

            AppsServiceFailedRequest.Setup(appService => 
                appService.IsRequestValidOnThisLicense(
                    It.IsAny<string>(), 
                    It.IsAny<int>(), 
                    It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            AppsServiceFailedRequest.Setup(appService =>
                appService.IsOwnerOfThisLicense(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            AppsServiceFailedRequest.Setup(appService => 
                appService.UpdateApp(It.IsAny<AppRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error updating app"
                }));

            AppsServiceFailedRequest.Setup(appService => 
                appService.GetAppUsers(It.IsAny<BaseRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult() {

                    Success = false,
                    Message = "Error retrieving app users",
                    Users = new List<User>()
                }));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetLicense(It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseResult() {

                    License = string.Empty,
                    Success = false,
                    Message = "App not found"
                }));

            AppsServiceFailedRequest.Setup(appService => 
                appService.AddAppUser(It.IsAny<int>(), It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error adding user to app"
                }));

            AppsServiceFailedRequest.Setup(appService => 
                appService.RemoveAppUser(It.IsAny<int>(), It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error removing user from app"
                }));

            AppsServiceFailedRequest.Setup(appService => 
                appService.ActivateApp(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error activating app"
                }));

            AppsServiceFailedRequest.Setup(appService => 
                appService.DeactivateApp(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error deactivating app"
                }));

            AppsServiceFailedRequest.Setup(appService =>
                appService.DeleteApp(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error deleting app"
                }));

            AppsServiceInvalidRequest.Setup(appService => 
                appService.IsRequestValidOnThisLicense(
                    It.IsAny<string>(), 
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(false));
        }
    }
}
