using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Test.MockServices;
using SudokuCollective.Test.TestData;
using SudokuCollective.Api.Controllers;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Test.TestCases.Controllers
{
    public class LicenseControllerShould
    {
        private DatabaseContext context;
        private LicensesController sutSuccess;
        private LicensesController sutFailure;
        private MockAppsService mockAppsService;
        private BaseRequest baseRequest;
        private AppRequest appRequest;

        [SetUp]
        public async Task Setup()
        {
            context = await TestDatabase.GetDatabaseContext();
            mockAppsService = new MockAppsService(context);

            baseRequest = new BaseRequest();

            appRequest = new AppRequest()
            {
                Name = "New Test App 3",
                DevUrl = "https://localhost:8080",
                LiveUrl = "https://TestApp3.com",
                License = TestObjects.GetLicense(),
                RequestorId = 1,
                PageListModel = new PageListModel()
            };

            sutSuccess = new LicensesController(mockAppsService.AppsServiceSuccessfulRequest.Object);
            sutFailure = new LicensesController(mockAppsService.AppsServiceFailedRequest.Object);
        }

        [Test]
        [Category("Controllers")]
        public void SuccessfullyGetLicense()
        {
            // Arrange
            var appId = 1;

            // Act
            var result = sutSuccess.GetLicense(appId);
            var message = ((LicenseResult)((OkObjectResult)result.Result).Value).Message;
            var statusCode = ((OkObjectResult)result.Result).StatusCode;
            var license = ((LicenseResult)((OkObjectResult)result.Result).Value).License;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 200: App Found"));
            Assert.That(statusCode, Is.EqualTo(200));
            Assert.That(license, Is.InstanceOf<string>());
        }

        [Test]
        [Category("Controllers")]
        public void IssueErrorAndMessageShouldGetLicenseFail()
        {
            // Arrange
            var appId = 1;

            // Act
            var result = sutFailure.GetLicense(appId);
            var message = ((LicenseResult)((NotFoundObjectResult)result.Result).Value).Message;
            var statusCode = ((NotFoundObjectResult)result.Result).StatusCode;

            // Assert
            Assert.That(result.Result, Is.InstanceOf<ActionResult>());
            Assert.That(message, Is.EqualTo("Status Code 404: App Not Found"));
            Assert.That(statusCode, Is.EqualTo(404));
        }
    }
}