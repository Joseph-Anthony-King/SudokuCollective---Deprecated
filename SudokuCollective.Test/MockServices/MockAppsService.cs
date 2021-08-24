using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.MockRepositories;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;

namespace SudokuCollective.Test.MockServices
{
    public class MockAppsService
    {
        internal MockAppsRepository MockAppsRepository { get; set; }
        internal MockUsersRepository MockUsersRepository { get; set; }
        internal MockAppAdminsRepository MockAppAdminsRepository { get; set; }

        internal Mock<IAppsService> AppsServiceSuccessfulRequest { get; set; }
        internal Mock<IAppsService> AppsServiceFailedRequest { get; set; }
        internal Mock<IAppsService> AppsServiceInvalidRequest { get; set; }
        internal Mock<IAppsService> AppsServicePromoteUserFailsRequest { get; set; }

        public MockAppsService(DatabaseContext context)
        {
            MockAppsRepository = new MockAppsRepository(context);
            MockUsersRepository = new MockUsersRepository(context);
            MockAppAdminsRepository = new MockAppAdminsRepository(context);

            AppsServiceSuccessfulRequest = new Mock<IAppsService>();
            AppsServiceFailedRequest = new Mock<IAppsService>();
            AppsServiceInvalidRequest = new Mock<IAppsService>();
            AppsServicePromoteUserFailsRequest = new Mock<IAppsService>();

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.Get(
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppFoundMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Object
                } as IAppResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetAppByLicense(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetByLicense(It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppFoundMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Object
                } as IAppResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetApps(
                    It.IsAny<Paginator>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsFoundMessage,
                    Apps = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Objects
                        .ConvertAll(a => (IApp)a)
                } as IAppsResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetMyApps(
                    It.IsAny<int>(), 
                    It.IsAny<Paginator>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsFoundMessage,
                    Apps = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Objects
                        .ConvertAll(a => (IApp)a)
                } as IAppsResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetRegisteredApps(
                    It.IsAny<int>(),
                    It.IsAny<Paginator>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsFoundMessage,
                    Apps = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Objects
                        .ConvertAll(a => (IApp)a)
                } as IAppsResult));

            AppsServiceSuccessfulRequest.Setup(appsService =>
                appsService.Create(It.IsAny<ILicenseRequest>()))
                .Returns(Task.FromResult(new AppResult() 
                { 
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppCreatedMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<App>())
                        .Result
                        .Object
                } as IAppResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.IsRequestValidOnThisLicense(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.IsOwnerOfThisLicense(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.Update(It.IsAny<int>(), It.IsAny<AppRequest>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppUpdatedMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<App>())
                        .Result
                        .Object,
                } as IAppResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetAppUsers(
                    It.IsAny<int>(), 
                    It.IsAny<int>(),
                    It.IsAny<Paginator>(),
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAppUsers(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = UsersMessages.UsersFoundMessage,
                    Users = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAppUsers(It.IsAny<int>())
                        .Result
                        .Objects
                        .ConvertAll(u => (IUser)u)
                } as IUsersResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.GetLicense(It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseResult()
                {
                    IsSuccess = true,
                    Message = AppsMessages.AppFoundMessage,
                    License = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetLicense(It.IsAny<int>())
                        .Result
                } as ILicenseResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.AddAppUser(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .AddAppUser(It.IsAny<int>(), It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.UserAddedToAppMessage
                } as IBaseResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.RemoveAppUser(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .RemoveAppUser(It.IsAny<int>(), It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.UserRemovedFromAppMessage
                } as IBaseResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.Activate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Activate(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppActivatedMessage
                } as IBaseResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.Deactivate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Deactivate(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppDeactivatedMessage
                } as IBaseResult));

            AppsServiceSuccessfulRequest.Setup(appService =>
                appService.DeleteOrReset(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Delete(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppDeletedMessage
                } as IBaseResult));

            AppsServiceSuccessfulRequest.Setup(appsService =>
                appsService.ActivateAdminPrivileges(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    IsSuccess = MockAppAdminsRepository
                        .AppAdminsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserHasBeenPromotedToAdminMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            AppsServiceSuccessfulRequest.Setup(appsService =>
                appsService.DeactivateAdminPrivileges(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    IsSuccess = MockAppAdminsRepository
                        .AppAdminsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AdminPrivilegesDeactivatedMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.Get(
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppNotFoundMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Object,
                } as IAppResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetAppByLicense(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .GetByLicense(It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppNotFoundMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .GetByLicense(It.IsAny<string>())
                        .Result
                        .Object,
                } as IAppResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetApps(
                    It.IsAny<Paginator>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsNotFoundMessage,
                    Apps = null
                } as IAppsResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetMyApps(
                    It.IsAny<int>(), 
                    It.IsAny<Paginator>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsNotFoundMessage,
                    Apps = null
                } as IAppsResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetRegisteredApps(
                    It.IsAny<int>(),
                    It.IsAny<Paginator>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsNotFoundMessage,
                    Apps = null
                } as IAppsResult));

            AppsServiceFailedRequest.Setup(appsService =>
                appsService.Create(It.IsAny<ILicenseRequest>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppNotCreatedMessage,
                    App = null
                } as IAppResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.IsRequestValidOnThisLicense(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            AppsServiceFailedRequest.Setup(appService =>
                appService.IsOwnerOfThisLicense(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            AppsServiceFailedRequest.Setup(appService =>
                appService.Update(It.IsAny<int>(), It.IsAny<AppRequest>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppNotUpdatedMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<App>())
                        .Result
                        .Object
                } as IAppResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetAppUsers(
                    It.IsAny<int>(),
                    It.IsAny<int>(), 
                    It.IsAny<Paginator>(),
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .GetAppUsers(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = UsersMessages.UsersNotFoundMessage,
                    Users = null
                } as IUsersResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.GetLicense(It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseResult()
                {
                    IsSuccess = false,
                    Message = AppsMessages.AppNotFoundMessage,
                    License = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .GetLicense(It.IsAny<int>())
                        .Result
                } as ILicenseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.AddAppUser(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .AddAppUser(It.IsAny<int>(), It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.UserNotAddedToAppMessage
                } as IBaseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.RemoveAppUser(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .RemoveAppUser(It.IsAny<int>(), It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.UserNotRemovedFromAppMessage
                } as IBaseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.Activate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .Activate(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppNotActivatedMessage
                } as IBaseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.Deactivate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .Deactivate(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppNotDeactivatedMessage
                } as IBaseResult));

            AppsServiceFailedRequest.Setup(appService =>
                appService.DeleteOrReset(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositoryFailedRequest
                        .Object
                        .Delete(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = "Error deleting app"
                } as IBaseResult));

            AppsServiceFailedRequest.Setup(appsService =>
                appsService.ActivateAdminPrivileges(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    IsSuccess = MockAppAdminsRepository
                        .AppAdminsRepositoryFailedRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserHasNotBeenPromotedToAdminMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            AppsServiceFailedRequest.Setup(appsService =>
                appsService.DeactivateAdminPrivileges(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    IsSuccess = MockAppAdminsRepository
                        .AppAdminsRepositoryFailedRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.DeactivationOfAdminPrivilegesFailedMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.Get(
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppFoundMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Object
                } as IAppResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.GetAppByLicense(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetByLicense(It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppFoundMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Object
                } as IAppResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.GetApps(
                    It.IsAny<Paginator>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsFoundMessage,
                    Apps = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Objects
                        .ConvertAll(a => (IApp)a)
                } as IAppsResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.GetMyApps(
                    It.IsAny<int>(), 
                    It.IsAny<Paginator>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsFoundMessage,
                    Apps = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Objects
                        .ConvertAll(a => (IApp)a)
                } as IAppsResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.GetRegisteredApps(
                    It.IsAny<int>(),
                    It.IsAny<Paginator>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsFoundMessage,
                    Apps = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Objects
                        .ConvertAll(a => (IApp)a)
                } as IAppsResult));

            AppsServiceInvalidRequest.Setup(appsService =>
                appsService.Create(It.IsAny<ILicenseRequest>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppCreatedMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<App>())
                        .Result
                        .Object
                } as IAppResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.IsRequestValidOnThisLicense(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(false));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.IsOwnerOfThisLicense(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.Update(It.IsAny<int>(), It.IsAny<AppRequest>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppUpdatedMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<App>())
                        .Result
                        .Object,
                } as IAppResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.GetAppUsers(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Paginator>(),
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAppUsers(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = UsersMessages.UsersFoundMessage,
                    Users = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAppUsers(It.IsAny<int>())
                        .Result
                        .Objects
                        .ConvertAll(u => (IUser)u)
                } as IUsersResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.GetLicense(It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseResult()
                {
                    IsSuccess = true,
                    Message = AppsMessages.AppFoundMessage,
                    License = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetLicense(It.IsAny<int>())
                        .Result
                } as ILicenseResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.AddAppUser(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .AddAppUser(It.IsAny<int>(), It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.UserAddedToAppMessage
                } as IBaseResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.RemoveAppUser(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .RemoveAppUser(It.IsAny<int>(), It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.UserRemovedFromAppMessage
                } as IBaseResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.Activate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Activate(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppActivatedMessage
                } as IBaseResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.Deactivate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Deactivate(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppDeactivatedMessage
                } as IBaseResult));

            AppsServiceInvalidRequest.Setup(appService =>
                appService.DeleteOrReset(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Delete(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppDeletedMessage
                } as IBaseResult));

            AppsServiceInvalidRequest.Setup(appsService =>
                appsService.ActivateAdminPrivileges(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    IsSuccess = MockAppAdminsRepository
                        .AppAdminsRepositoryFailedRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppNotFoundMessage,
                    User = new User()
                } as IUserResult));

            AppsServiceInvalidRequest.Setup(appsService =>
                appsService.DeactivateAdminPrivileges(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    IsSuccess = MockAppAdminsRepository
                        .AppAdminsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AdminPrivilegesDeactivatedMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.Get(
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppFoundMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Object
                } as IAppResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.GetAppByLicense(
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetByLicense(It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppFoundMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Object
                } as IAppResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.GetApps(
                    It.IsAny<Paginator>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsFoundMessage,
                    Apps = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Objects
                        .ConvertAll(a => (IApp)a)
                } as IAppsResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.GetMyApps(
                    It.IsAny<int>(), 
                    It.IsAny<Paginator>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsFoundMessage,
                    Apps = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Objects
                        .ConvertAll(a => (IApp)a)
                } as IAppsResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.GetRegisteredApps(
                    It.IsAny<int>(),
                    It.IsAny<Paginator>()))
                .Returns(Task.FromResult(new AppsResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = AppsMessages.AppsFoundMessage,
                    Apps = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Objects
                        .ConvertAll(a => (IApp)a)
                } as IAppsResult));

            AppsServicePromoteUserFailsRequest.Setup(appsService =>
                appsService.Create(It.IsAny<ILicenseRequest>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppCreatedMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<App>())
                        .Result
                        .Object
                } as IAppResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.IsRequestValidOnThisLicense(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.IsOwnerOfThisLicense(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.Update(It.IsAny<int>(), It.IsAny<AppRequest>()))
                .Returns(Task.FromResult(new AppResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppUpdatedMessage,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<App>())
                        .Result
                        .Object,
                } as IAppResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.GetAppUsers(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<Paginator>(),
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAppUsers(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = UsersMessages.UsersFoundMessage,
                    Users = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetAppUsers(It.IsAny<int>())
                        .Result
                        .Objects
                        .ConvertAll(u => (IUser)u)
                } as IUsersResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.GetLicense(It.IsAny<int>()))
                .Returns(Task.FromResult(new LicenseResult()
                {
                    IsSuccess = true,
                    Message = AppsMessages.AppFoundMessage,
                    License = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetLicense(It.IsAny<int>())
                        .Result
                } as ILicenseResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.AddAppUser(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .AddAppUser(It.IsAny<int>(), It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.UserAddedToAppMessage
                } as IBaseResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.RemoveAppUser(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .RemoveAppUser(It.IsAny<int>(), It.IsAny<string>())
                        .Result
                        .Success,
                    Message = AppsMessages.UserRemovedFromAppMessage
                } as IBaseResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.Activate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Activate(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppActivatedMessage
                } as IBaseResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.Deactivate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Deactivate(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppDeactivatedMessage
                } as IBaseResult));

            AppsServicePromoteUserFailsRequest.Setup(appService =>
                appService.DeleteOrReset(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    IsSuccess = MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .Delete(It.IsAny<App>())
                        .Result
                        .Success,
                    Message = AppsMessages.AppDeletedMessage
                } as IBaseResult));

            AppsServicePromoteUserFailsRequest.Setup(appsService =>
                appsService.ActivateAdminPrivileges(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    IsSuccess = MockAppAdminsRepository
                        .AppAdminsRepositoryFailedRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserHasNotBeenPromotedToAdminMessage,
                    User = new User()
                } as IUserResult));

            AppsServicePromoteUserFailsRequest.Setup(appsService =>
                appsService.ActivateAdminPrivileges(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    IsSuccess = MockAppAdminsRepository
                        .AppAdminsRepositoryFailedRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserHasNotBeenPromotedToAdminMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            AppsServicePromoteUserFailsRequest.Setup(appsService =>
                appsService.DeactivateAdminPrivileges(It.IsAny<int>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    IsSuccess = MockAppAdminsRepository
                        .AppAdminsRepositoryFailedRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = AppsMessages.DeactivationOfAdminPrivilegesFailedMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));
        }
    }
}
