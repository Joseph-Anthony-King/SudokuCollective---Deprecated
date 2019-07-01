using System;
using Moq;
using Moq.Protected;
using NUnit.Framework;
using SudokuApp.Models;
using SudokuApp.Models.Interfaces;

namespace SudokuApp.Tests {

    public class UserShould {

        [Test]
        public void AcceptFirstAndLastName() {

            var sut = new User("John", "Doe");

            Assert.That(sut.FirstName, Is.EqualTo("John"));
            Assert.That(sut.LastName, Is.EqualTo("Doe"));
            Assert.That(sut.FullName, Is.EqualTo("John Doe"));
        }

        [Test]
        public void HaveAccurateDateCreated() {

            var sut = new User();
            var currentDateTime = DateTime.Now;

            Assert.That(sut.DateCreated, Is.EqualTo(currentDateTime).Within(1).Seconds);
        }

        [Test]
        public void HavingWorkingProperties() {

            var sut = new User();

            sut.NickName = "Johnny";
            sut.Email = "JohnDoe@example.com";

            Assert.That(sut.NickName, Is.EqualTo("Johnny"));
            Assert.That(sut.Email, Is.EqualTo("JohnDoe@example.com"));
        }

        [Test]
        public void HaveAnID() {

            var sut = new User();

            Assert.That(sut.Id, Is.TypeOf<int>());
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        public void HaveAGameList() {

            var sut = new User();

            Assert.That(sut.Games.Count, Is.EqualTo(0));
        }
    }
}
