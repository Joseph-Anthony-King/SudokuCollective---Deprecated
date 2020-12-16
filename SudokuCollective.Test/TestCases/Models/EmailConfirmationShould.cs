using NUnit.Framework;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.TestData;
using System;
using System.Collections.Generic;
using System.Text;

namespace SudokuCollective.Test.TestCases.Models
{
    public class EmailConfirmationShould
    {
        [Test]
        [Category("Models")]
        public void ImplementIDBEntry()
        {
            // Arrange and Act
            var sut = TestObjects.GetNewEmailConfirmation();

            // Assert
            Assert.That(sut, Is.InstanceOf<IEntityBase>());
        }

        [Test]
        [Category("Models")]
        public void DistinguishBetweenNewAndUpdateRequests()
        {
            // Arrange and Act
            var sutNewEmailConfirmation = TestObjects.GetNewEmailConfirmation();
            var sutUpdateEmailConfirmation = TestObjects.GetUpdateEmailConfirmation();

            // Assert
            Assert.That(sutNewEmailConfirmation.IsUpdate, Is.False);
            Assert.That(sutUpdateEmailConfirmation.IsUpdate, Is.True);
        }
    }
}
