using System.Threading.Tasks;
using NUnit.Framework;
using SudokuCollective.Domain.Models;
using SudokuCollective.Tests.TestData;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Services;

namespace SudokuCollective.Tests.TestCases.Services {

    public class AppServiceShould {

        private DatabaseContext _context;

        [SetUp]
        public async Task Setup() {

            _context = await TestDatabase.GetDatabaseContext();
        }

        [Test]
        [Category("Services")]
        public async Task GetAppByID() {

            // Arrange
            var sut = new AppsService(_context);

            // Act
            var result = await sut.GetApp(1);

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.App, Is.TypeOf<App>());
        }

        [Test]
        [Category("Services")]
        public async Task GetAppByIDReturnsFalseIfNotFound()
        {

            // Arrange
            var sut = new AppsService(_context);

            // Act
            var result = await sut.GetApp(3);

            // Assert
            Assert.That(result.Success, Is.False);
            Assert.That(result.App.IsActive, Is.False);
        }

        [Test]
        [Category("Services")]
        public async Task GetMultipleApps() {

            // Arrange
            var sut = new AppsService(_context);

            // Act
            var result = await sut.GetApps(new PageListModel());

            // Assert
            Assert.That(result.Success, Is.True);
            Assert.That(result.Apps.Count, Is.EqualTo(2));
        }
    }
}
