using System;
using System.Linq;
using NUnit.Framework;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;

namespace SudokuCollective.Tests {

    public class UserShould {

        [Test]
        [Category("Models")]
        public void HaveAnID() {

            // Arrange and Act
            var sut = new User();

            // Assert
            Assert.That(sut.Id, Is.TypeOf<int>());
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void AcceptFirstAndLastName() {

            // Arrange and Act
            var sut = new User(
                "John", 
                "Doe", 
                "Password");

            // Assert
            Assert.That(sut.FirstName, Is.EqualTo("John"));
            Assert.That(sut.LastName, Is.EqualTo("Doe"));
            Assert.That(sut.FullName, Is.EqualTo("John Doe"));
            Assert.That(sut.Password, Is.EqualTo("Password"));
        }

        [Test]
        [Category("Models")]
        public void HaveWorkingProperties() {

            // Arrange
            var sut = new User();

            // Act
            sut.NickName = "Johnny";
            sut.Email = "JohnDoe@example.com";

            // Assert
            Assert.That(sut.NickName, Is.EqualTo("Johnny"));
            Assert.That(sut.Email, Is.EqualTo("JohnDoe@example.com"));
        }

        [Test]
        [Category("Models")]
        public void HaveAccurateDateCreated() {

            // Arrange and Act
            var sut = new User();
            var currentDateTime = DateTime.UtcNow;

            // Assert
            Assert.That(sut.DateCreated, Is.EqualTo(currentDateTime).Within(1).Seconds);
        }

        [Test]
        [Category("Models")]
        public void HaveAGameList() {

            // Arrange and Act
            var sut = new User();

            // Assert
            Assert.That(sut.Games.Count, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void HaveAPassword() {

            // Arrange and Act
            var sut = new User();

            // Assert
            Assert.That(sut.Password, Is.TypeOf<string>());
        }
    }
}
