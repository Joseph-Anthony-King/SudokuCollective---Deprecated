using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;
using SudokuCollective.Test.TestData;

namespace SudokuCollective.Test.MockRepositories
{
    public class MockAppsRepository
    {
        private readonly DatabaseContext context;
        internal Mock<IAppsRepository<App>> AppsRepositorySuccessfulRequest { get; set; }
        internal Mock<IAppsRepository<App>> AppsRepositoryFailedRequest { get; set; }

        public MockAppsRepository(DatabaseContext ctxt)
        {
            context = ctxt;
            var todaysDate = DateTime.UtcNow;

            AppsRepositorySuccessfulRequest = new Mock<IAppsRepository<App>>();
            AppsRepositoryFailedRequest = new Mock<IAppsRepository<App>>();

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.Create(It.IsAny<App>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = new App(
                            3,
                            "Test App 3",
                            "2b789e72-1df3-4313-8091-68cfa8a1db60",
                            1,
                            "https://localhost:8080",
                            "https://testapp3.com",
                            true,
                            false,
                            true,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            string.Empty,
                            todaysDate,
                            DateTime.MinValue)
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Apps.FirstOrDefault(a => a.Id == 1)
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.GetByLicense(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Apps.FirstOrDefault(a => a.Id == 1)
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Objects = context.Apps.ToList().ConvertAll(a => (IEntityBase)a)
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.GetAppUsers(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Objects = context.Users
                            .Where(user => user.Apps.Any(ua => ua.AppId == 1))
                            .ToList()
                            .ConvertAll(a => (IEntityBase)a)
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.Update(It.IsAny<App>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Object = context.Apps.FirstOrDefault(a => a.Id == 1)
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo => 
                appsRepo.UpdateRange(It.IsAny<List<App>>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.AddAppUser(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.RemoveAppUser(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.Delete(It.IsAny<App>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.DeleteRange(It.IsAny<List<App>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.Reset(It.IsAny<App>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.Activate(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.Deactivate(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.IsAppLicenseValid(It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.GetLicense(It.IsAny<int>()))
                    .Returns(Task.FromResult(
                        TestObjects.GetLicense()));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.IsUserRegisteredToApp(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            AppsRepositorySuccessfulRequest.Setup(appsRepo =>
                appsRepo.IsUserOwnerOfApp(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.Create(It.IsAny<App>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false,
                        Object = null
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false,
                        Object = null
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.GetByLicense(It.IsAny<string>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false,
                        Object = null
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false,
                        Objects = null
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.GetAppUsers(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false,
                        Objects = null
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.Update(It.IsAny<App>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false,
                        Object = null
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.UpdateRange(It.IsAny<List<App>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.AddAppUser(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.RemoveAppUser(It.IsAny<int>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.Delete(It.IsAny<App>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.DeleteRange(It.IsAny<List<App>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.Reset(It.IsAny<App>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.Activate(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.Deactivate(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.IsAppLicenseValid(It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.GetLicense(It.IsAny<int>()))
                    .Returns(Task.FromResult(string.Empty));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.IsUserRegisteredToApp(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            AppsRepositoryFailedRequest.Setup(appsRepo =>
                appsRepo.IsUserOwnerOfApp(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(false));
        }
    }
}
