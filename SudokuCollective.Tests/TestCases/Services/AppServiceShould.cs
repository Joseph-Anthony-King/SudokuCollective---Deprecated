using System;
using System.Linq;
using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.AppRequests;
using SudokuCollective.WebApi.Services;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.TestCases.Services {

    public class AppServiceShould {

        private DatabaseContext _context;
        private IAppsService sut;
        private DateTime dateCreated;
        private string license;
        private BaseRequest baseRequest;

        [SetUp]
        public async Task Setup() {

            _context = await TestDatabase.GetDatabaseContext();
            sut = new AppsService(_context);
            dateCreated = DateTime.UtcNow;
            license = "D17F0ED3-BE9A-450A-A146-F6733DB2BBDB";
            baseRequest = new BaseRequest() {
             
                License = license,
                RequestorId = 1
            };
        }

        [Test]
        [Category("Services")]
        public async Task GetAppByID() {

            // Arrange

            // Act
            var result = await sut.GetApp(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.App, Is.TypeOf<App>());
        }

        [Test]
        [Category("Services")]
        public async Task GetAppByIDReturnsFalseIfNotFound() {

            // Arrange

            // Act
            var result = await sut.GetApp(3);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("App not found"));
            Assert.That(result.App.IsActive, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task GetMultipleApps() {

            // Arrange

            // Act
            var result = await sut.GetApps(new PageListModel());

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Apps.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task CreateApps() {

            // Arrange

            // Act
            var result = await sut.CreateApp(new LicenseRequest() {

                Name = "Test App 3",
                OwnerId = 1,
                DevUrl = "https://localhost:5001",
                LiveUrl = "https://testapp3.com"
            });

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.App.Id, Is.TypeOf<int>());
            Assert.That(result.App.IsActive, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task NotCreateAppsIfOwnerDoesNotExist() {

            // Arrange

            // Act
            var result = await sut.CreateApp(new LicenseRequest() {

                Name = "Test App 3",
                OwnerId = 4,
                DevUrl = "https://localhost:5001",
                LiveUrl = "https://testapp3.com"
            });

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("Intended owner id does not exist"));
        }

        [Test]
        [Category("Services")]
        public async Task GetAppByLicense() {

            // Arrange

            // Act
            var result = await sut.GetAppByLicense(license);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.App.Id, Is.EqualTo(1));
            Assert.That(result.App, Is.TypeOf<App>());
        }

        [Test]
        [Category("Services")]
        public async Task NotGetAppByLicenseIfInvalid() {

            // Arrange
            var invalidLicense = "5CDBFC8F-F304-4703-831B-750A7B7F8531";

            // Act
            var result = await sut.GetAppByLicense(invalidLicense);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("App not found"));
        }

        [Test]
        [Category("Services")]
        public async Task RetrieveLicense() {

            // Arrange

            // Act
            var result = await sut.GetLicense(1);

            // Assert
            Assert.That(result.License, Is.EqualTo(license));
        }

        [Test]
        [Category("Services")]
        public async Task NotRetrieveLicenseIfAppDoesNotExist() {

            // Arrange

            // Act
            var result = await sut.GetLicense(5);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.Message, Is.EqualTo("App not found"));
            Assert.That(result.License, Is.Not.EqualTo(license));
        }

        [Test]
        [Category("Services")]
        public async Task GetUsersByApp() {

            // Arrange

            // Act
            var result = await sut.GetAppUsers(baseRequest);

            // Assert
            Assert.That(result.Users.Count, Is.EqualTo(2));
        }

        [Test]
        [Category("Services")]
        public async Task UpdateApps() {

            // Arrange

            // Act
            var result = await sut.UpdateApp(
                new AppRequest() {

                    License = license,
                    RequestorId = 1,
                    PageListModel = new PageListModel(),
                    Name = "Test App 1... UPDATED!",
                    DevUrl = "https://localhost:4200",
                    LiveUrl = "https://testapp.com"
                }
            );

            var checkAppName = _context.Apps.FirstOrDefault(predicate: a => a.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(checkAppName.Name, Is.EqualTo("Test App 1... UPDATED!"));
        }

        [Test]
        [Category("Services")]
        public async Task AddUsersToApp() {

            // Arrange
            var user = new User() {

                Id = 3,
                UserName = "TestUser3",
                FirstName = "John",
                LastName = "Doe",
                NickName = "Johnny Boy",
                Email = "testuser3@example.com",
                Password = "password1",
                IsActive = true,
                DateCreated = dateCreated,
                DateUpdated = dateCreated
            };

            var userRole = new UserRole() {

                UserId = 3,
                RoleId = 4
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            await _context.UsersRoles.AddAsync(userRole);
            await _context.SaveChangesAsync();

            // Act
            var result = await sut.AddAppUser(3, baseRequest);

            var appUsers = _context.Users
                .Where(u => u.Apps.Any(app => app.AppId == 1)).ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(appUsers.Count, Is.EqualTo(3));
        }

        [Test]
        [Category("Services")]
        public async Task RemoveUsersFromApps() {

            // Arrange

            // Act
            var result = await sut.RemoveAppUser(2, baseRequest);

            var appUsers = _context.Users
                .Where(u => u.Apps.Any(app => app.AppId == 1)).ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(appUsers.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("Services")]
        public async Task DeleteApps() {

            // Arrange

            // Act
            var result = await sut.DeleteApp(2);
            var confirmAppCount = _context.Apps.ToList();

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(confirmAppCount.Count, Is.EqualTo(1));
        }

        [Test]
        [Category("Services")]
        public async Task ActivateApps() {

            // Arrange

            // Act
            var result = await sut.ActivateApp(1);

            var confrirmAppStatus = _context.Apps
                .FirstOrDefault(predicate: a => a.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(confrirmAppStatus.IsActive, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task DeactivateApps() {

            // Arrange

            // Act
            var result = await sut.DeactivateApp(1);

            var confrirmAppStatus = _context.Apps
                .FirstOrDefault(predicate: a => a.Id == 1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(confrirmAppStatus.IsActive, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task PermitValidRequests() {

            // Arrange

            // Act
            var result = await sut.IsRequestValidOnThisLicense(license, 1);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [Category("Services")]
        public async Task DenyInvalidRequests() {

            // Arrange
            var invalidLicense = "5CDBFC8F-F304-4703-831B-750A7B7F8531";

            // Act
            var result = await sut.IsRequestValidOnThisLicense(invalidLicense, 1);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task PermitSuperUserSystemWideAccess() {

            // Arrange
            var newAppResult = await sut.CreateApp(new LicenseRequest() {

                Name = "Test App 3",
                OwnerId = 2,
                DevUrl = "https://localhost:5001",
                LiveUrl = "https://testapp3.com"
            });

            var license = _context.Apps
                .Where(a => a.Id == 3)
                .Select(a => a.License)
                .FirstOrDefault();

            // Act
            var superUserIsInApp = newAppResult.App.Users
                .Any(ua => ua.UserId == 1);

            var result = await sut.IsRequestValidOnThisLicense(license, 1);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(superUserIsInApp, Is.False);
        }
    }
}
