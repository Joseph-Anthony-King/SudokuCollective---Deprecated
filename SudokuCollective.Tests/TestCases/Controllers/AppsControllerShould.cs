using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Controllers;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.AppRequests;
using SudokuCollective.WebApi.Models.ResultModels.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Controllers {

    public class AppsControllerShould {

        private DatabaseContext context;
        private AppsController sut;
        private Mock<IAppsService> mockAppsService;
        private BaseRequest baseRequest;
        private AppRequest appRequest;

        [SetUp]
        public async Task Setup() {

            context = await TestDatabase.GetDatabaseContext();

            baseRequest = new BaseRequest();
            mockAppsService = new Mock<IAppsService>();

            mockAppsService.Setup(appService => 
                appService.GetApp(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppResult() {
            
                        Success = true,
                        Message = string.Empty,
                        App = new App()
                        {
                            Id = 1,
                            Name = "Test App 1",
                            License = TestObjects.GetLicense(),
                            OwnerId = 1,
                            LiveUrl = "https://localhost:4200",
                            DevUrl = "https://testapp.com",
                            IsActive = true,
                            DateCreated = DateTime.UtcNow,
                            DateUpdated = DateTime.MinValue
                        }
                    }));

            mockAppsService.Setup(appService =>
                appService.GetAppByLicense(It.IsAny<string>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppResult()
                {

                    Success = true,
                    Message = string.Empty,
                    App = new App()
                    {
                        Id = 1,
                        Name = "Test App 1",
                        License = TestObjects.GetLicense(),
                        OwnerId = 1,
                        LiveUrl = "https://localhost:4200",
                        DevUrl = "https://testapp.com",
                        IsActive = true,
                        DateCreated = DateTime.UtcNow,
                        DateUpdated = DateTime.MinValue
                    }
                }));

            mockAppsService.Setup(appService =>
                appService.GetApps(It.IsAny<PageListModel>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new AppsResult() {

                    Success = true,
                    Message = string.Empty,
                    Apps = new List<App>()
                }));

            mockAppsService.Setup(appService => 
                appService.IsRequestValidOnThisLicense(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            mockAppsService.Setup(appService => 
                appService.UpdateApp(It.IsAny<AppRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            mockAppsService.Setup(appService => 
                appService.GetAppUsers(It.IsAny<BaseRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult() {

                    Success = true,
                    Message = string.Empty,
                    Users = context.Users
                        .Where(user => user.Apps.Any(userApp => userApp.AppId == 1))
                        .ToList()
                }));

            mockAppsService.Setup(appService => 
                appService.AddAppUser(It.IsAny<int>(), It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            mockAppsService.Setup(appService => 
                appService.RemoveAppUser(It.IsAny<int>(), It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            mockAppsService.Setup(appService => 
                appService.ActivateApp(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            mockAppsService.Setup(appService => 
                appService.DeactivateApp(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            appRequest = new AppRequest() {

                Name = "New Test App 3",
                DevUrl = "https://localhost:8080",
                LiveUrl = "https://TestApp3.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            sut = new AppsController(mockAppsService.Object);
        }

        [Test]
        [Category("Controllers")]
        public void GetAnApp() {

            // Arrange
            var appId = 1;

            // Act
            var result = sut.GetApp(appId, baseRequest);
            var processResult = result.Result;
            var app = ((OkObjectResult)processResult.Result).Value;
            var statusCode = ((OkObjectResult)processResult.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(app, Is.InstanceOf<App>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void GetAnAppByLicense() {

            // Arrange

            // Act
            var result = sut.GetAppByLicense(baseRequest, true);
            var processResult = result.Result;
            var app = ((OkObjectResult)processResult.Result).Value;
            var statusCode = ((OkObjectResult)processResult.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<App>>());
            Assert.That(app, Is.InstanceOf<App>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void GetApps() {

            // Arrange

            // Act
            var result = sut.GetApps(baseRequest, true);
            var processResult = result.Result;
            var apps = ((OkObjectResult)processResult.Result).Value;
            var statusCode = ((OkObjectResult)processResult.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(apps, Is.InstanceOf<List<App>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void UpdateApps() {

            // Arrange

            // Act
            var result = sut.UpdateApp(appRequest);
            var processResult = result.Result;
            var statusCode = ((OkResult)processResult).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void GetAppUsers() {

            // Arrange

            // Act
            var result = sut.GetUsers(baseRequest, true);
            var processResult = result.Result;
            var users = ((OkObjectResult)processResult.Result).Value;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<User>>>());
            Assert.That(((List<User>)users).Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Controllers")]
        public void AddUserToApp() {

            // Arrange

            // Act
            var result = sut.AddUser(3, baseRequest);
            var processResult = result.Result;
            var statusCode = ((OkResult)processResult).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void RemoveUserFromApp() {

            // Arrange

            // Act
            var result = sut.RemoveUser(3, baseRequest);
            var processResult = result.Result;
            var statusCode = ((OkResult)processResult).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void ActivateAnApp() {

            // Arrange

            // Act
            var result = sut.ActivateApp(1);
            var processResult = result.Result;
            var statusCode = ((OkResult)processResult).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }

        [Test]
        [Category("Controllers")]
        public void DeactivateAnApp() {

            // Arrange

            // Act
            var result = sut.DeactivateApp(1);
            var processResult = result.Result;
            var statusCode = ((OkResult)processResult).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(statusCode, Is.EqualTo(200));
        }
    }
}
