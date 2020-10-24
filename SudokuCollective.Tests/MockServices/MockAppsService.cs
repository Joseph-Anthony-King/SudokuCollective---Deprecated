using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.MockServices
{
    public class MockAppsService
    {
        private DatabaseContext _context;
        internal Mock<IAppsService> AppsServiceSuccessfulRequest { get; set; }
        internal Mock<IAppsService> AppsServiceFailedRequest { get; set; }
        internal Mock<IAppsService> AppsServiceInvalidRequest { get; set; }

        public MockAppsService(DatabaseContext context)
        {
            _context = context;
            AppsServiceSuccessfulRequest = new Mock<IAppsService>();
            AppsServiceFailedRequest = new Mock<IAppsService>();
            AppsServiceInvalidRequest = new Mock<IAppsService>();

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetApp(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    Success = true,
                    Message = string.Empty,
                    App = _context.Apps.FirstOrDefault(predicate: app => app.Id == 1)
                } as IAppResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetAppByLicense(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppResult()
                {

                    Success = true,
                    Message = string.Empty,
                    App = _context.Apps.FirstOrDefault(predicate: app => app.License.Equals(TestObjects.GetLicense()))
                } as IAppResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetApps(It.IsAny<PageListModel>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppsResult()
                {

                    Success = true,
                    Message = string.Empty,
                    Apps = (_context.Apps.ToList()).ConvertAll(a => a as IApp)
                } as IAppsResult));

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
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetAppUsers(It.IsAny<BaseRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {

                    Success = true,
                    Message = string.Empty,
                    Users = (context.Users
                        .Where(user => user.Apps.Any(userApp => userApp.AppId == 1))
                        .ToList()).ConvertAll(u => u as IUser)
                } as IUsersResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetLicense(It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseResult()
                {

                    License = TestObjects.GetLicense(),
                    Success = true,
                    Message = string.Empty
                } as ILicenseResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.AddAppUser(It.IsAny<int>(), It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.RemoveAppUser(It.IsAny<int>(), It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.ActivateApp(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.DeactivateApp(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.DeleteOrResetApp(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetApp(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppResult()
                {

                    Success = false,
                    Message = "Error retrieving app",
                    App = new App()
                } as IAppResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetAppByLicense(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppResult()
                {

                    Success = false,
                    Message = "Error retrieving app",
                    App = new App()
                } as IAppResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetApps(It.IsAny<PageListModel>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppsResult()
                {

                    Success = false,
                    Message = "Error retrieving apps",
                    Apps = new List<IApp>()
                } as IAppsResult));

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
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = false,
                    Message = "Error updating app"
                } as IBaseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetAppUsers(It.IsAny<BaseRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {

                    Success = false,
                    Message = "Error retrieving app users",
                    Users = new List<IUser>()
                } as IUsersResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetLicense(It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseResult()
                {

                    License = string.Empty,
                    Success = false,
                    Message = "App not found"
                } as ILicenseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.AddAppUser(It.IsAny<int>(), It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = false,
                    Message = "Error adding user to app"
                } as IBaseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.RemoveAppUser(It.IsAny<int>(), It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = false,
                    Message = "Error removing user from app"
                } as IBaseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.ActivateApp(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = false,
                    Message = "Error activating app"
                } as IBaseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.DeactivateApp(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = false,
                    Message = "Error deactivating app"
                } as IBaseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.DeleteOrResetApp(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new BaseResult()
                {

                    Success = false,
                    Message = "Error deleting app"
                } as IBaseResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.IsRequestValidOnThisLicense(
                    It.IsAny<string>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(false));
        }
    }
}
