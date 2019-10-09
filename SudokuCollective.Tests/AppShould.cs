using System;
using System.Collections.Generic;
using NUnit.Framework;
using SudokuCollective.Domain;
using SudokuCollective.Domain.Interfaces;

namespace SudokuCollective.Tests{

    public class AppShould {

        [Test]
        [Category("Models")]
        public void ImplementIDBEntry() {

            // Arrange and Act
            var sut = new App();

            // Assert
            Assert.That(sut, Is.InstanceOf<IEntityBase>());
        }

        [Test]
        [Category("Models")]
        public void HaveAnID() {

            // Arrange and Act
            var sut = new App();

            // Assert
            Assert.That(sut.Id, Is.TypeOf<int>());
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void HaveExpectedProperties() {

            // Arrange and Act
            var sut = new App();

            // Assert
            Assert.That(sut.Name, Is.TypeOf<string>());
            Assert.That(sut.License, Is.TypeOf<string>());
            Assert.That(sut.OwnerId, Is.TypeOf<int>());
            Assert.That(sut.DevUrl, Is.TypeOf<string>());
            Assert.That(sut.LiveUrl, Is.TypeOf<string>());
            Assert.That(sut.IsActive, Is.TypeOf<bool>());
            Assert.That(sut.DateCreated, Is.TypeOf<DateTime>());
            Assert.That(sut.DateUpdated, Is.TypeOf<DateTime>());
            Assert.That(sut.Users, Is.InstanceOf<ICollection<UserApp>>());
        }
        
        [Test]
        [Category("Models")]
        public void DefaultToActiveStatus() {

            // Arrange and Act
            var sut =  new App();

            // Assert
            Assert.That(sut.IsActive, Is.True);
        }
        
        [Test]
        [Category("Models")]
        public void ProvideLicenseByCallingGetLicense() {

            // Arrange and Act
            var sut =  new App();

            // Assert
            Assert.That(sut.GetLicense(0,0), Is.TypeOf<string>());
            Assert.That(sut.GetLicense(0,0), Is.EqualTo(sut.License));
        }
        
        [Test]
        [Category("Models")]
        public void HaveATrueActiveStatusIfActivateAppCalled() {

            // Arrange
            var sut =  new App();

            // Act
            sut.ActivateApp();

            // Assert
            Assert.That(sut.IsActive, Is.True);
        }
        
        [Test]
        [Category("Models")]
        public void HaveAFalseActiveStatusIfDeactivateAppCalled() {

            // Arrange
            var sut =  new App();

            // Act
            sut.DeactivateApp();

            // Assert
            Assert.That(sut.IsActive, Is.False);
        }
    }
}
