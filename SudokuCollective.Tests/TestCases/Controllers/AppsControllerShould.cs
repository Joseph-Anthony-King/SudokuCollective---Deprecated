using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Controllers;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.ResultModels.AppRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Controllers {

    public class AppsControllerShould {

        private AppsController sut;
        private Mock<IAppsService> mockAppsService;
        private BaseRequest baseRequest;

        [SetUp]
        public void Setup() {

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
                .Returns(Task.FromResult(new AppsResult()
                {

                    Success = true,
                    Message = string.Empty,
                    Apps = new List<App>()
                }));

            mockAppsService.Setup(appService => 
                appService.IsRequestValidOnThisLicense(It.IsAny<string>(), It.IsAny<int>()))
                .Returns(Task.FromResult(true));

            sut = new AppsController(mockAppsService.Object);
        }

        [Test]
        [Category("Controllers")]
        public void GetAnApp() {

            // Arrange
            var appId = 1;

            // Act
            var result = sut.GetApp(appId, baseRequest);
            var taskResult = result.Result;
            var app = ((OkObjectResult)taskResult.Result).Value;
            var statusCode = ((OkObjectResult)taskResult.Result).StatusCode;

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
            var taskResult = result.Result;
            var app = ((OkObjectResult)taskResult.Result).Value;
            var statusCode = ((OkObjectResult)taskResult.Result).StatusCode;

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
            var taskResult = result.Result;
            var apps = ((OkObjectResult)taskResult.Result).Value;
            var statusCode = ((OkObjectResult)taskResult.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult<IEnumerable<App>>>());
            Assert.That(apps, Is.InstanceOf<List<App>>());
            Assert.That(statusCode, Is.EqualTo(200));
        }
    }
}
