﻿using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Test.TestCases.Models
{
    public class SudokuCellShould
    {
        private ISudokuCell sut;
        private List<int> intList;
        private int firstInt;
        private SudokuMatrix populatedTestMatrix;

        [SetUp]
        public void Setup()
        {
            populatedTestMatrix = new SudokuMatrix();
            populatedTestMatrix.GenerateSolution();
            intList = populatedTestMatrix.ToIntList();
            firstInt = intList[0];

            populatedTestMatrix = new SudokuMatrix(intList);
        }

        [Test]
        [Category("Models")]
        public void ImplementIDBEntry()
        {
            // Arrange and Act
            sut = populatedTestMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut, Is.InstanceOf<IEntityBase>());
        }

        [Test]
        [Category("Models")]
        public void HaveAnID()
        {
            // Arrange and Act
            sut = populatedTestMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.Id, Is.TypeOf<int>());
            Assert.That(sut.Id, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void HaveAnAssociatedMatrix()
        {
            // Arrange and Act
            var testMatrix = new SudokuMatrix();
            sut = testMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.SudokuMatrixId, Is.TypeOf<int>());
            Assert.That(testMatrix.Id, Is.EqualTo(sut.SudokuMatrixId));
            Assert.That(testMatrix, Is.TypeOf<SudokuMatrix>());
        }

        [Test]
        [Category("Models")]
        public void SetCoordinatesInConstructor()
        {
            // Arrange and Act
            sut = populatedTestMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.Index, Is.EqualTo(1));
            Assert.That(sut.Column, Is.EqualTo(1));
            Assert.That(sut.Region, Is.EqualTo(1));
            Assert.That(sut.Row, Is.EqualTo(1));
        }

        [Test]
        [Category("Models")]
        public void HaveADefaultValueOfZero()
        {
            // Arrange and Act
            var testMatrix = new SudokuMatrix();
            sut = testMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.Value, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void BeObscuredByDefault()
        {
            // Arrange and Act
            sut = populatedTestMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.Hidden, Is.EqualTo(true));
            Assert.That(sut.DisplayedValue, Is.EqualTo(0));
            Assert.That(sut.DisplayedValue, Is.Not.EqualTo(sut.Value));
        }

        [Test]
        [Category("Models")]
        public void BeVisibleIfObscuredIsFalse()
        {
            // Arrange and Act
            sut = populatedTestMatrix.SudokuCells[0];
            sut.Hidden = false;

            // Assert
            Assert.That(firstInt, Is.Not.EqualTo(0));
            Assert.That(sut.DisplayedValue, Is.EqualTo(firstInt));
            Assert.That(sut.DisplayedValue, Is.EqualTo(sut.Value));
        }

        [Test]
        [Category("Models")]
        public void HaveAvailableValuesNineCountIfValueIsZero()
        {
            // Arrange and Act
            var testMatrix = new SudokuMatrix();
            sut = testMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.AvailableValues.Count, Is.EqualTo(9));
        }

        [Test]
        [Category("Models")]
        public void HaveAvailableValuesZeroCountIfValueNonZero()
        {
            // Arrange and Act
            sut = populatedTestMatrix.SudokuCells[0];

            // Assert
            Assert.That(sut.AvailableValues.Where(a => a.Available).Count, Is.EqualTo(0));
        }

        [Test]
        [Category("Models")]
        public void HaveToInt32OutputValueAsInt()
        {
            // Arrange and Act
            sut = populatedTestMatrix.SudokuCells[0];
            sut.Hidden = false;

            // Assert
            Assert.That(sut.ToInt32(), Is.TypeOf<int>());
            Assert.That(sut.ToInt32(), Is.EqualTo(firstInt));
        }

        [Test]
        [Category("Models")]
        public void HaveToStringOutputValueAsString()
        {
            // Arrange and Act
            sut = populatedTestMatrix.SudokuCells[0];
            sut.Hidden = false;

            // Assert
            Assert.That(sut.ToString(), Is.TypeOf<string>());
            Assert.That(sut.ToString(), Is.EqualTo(firstInt.ToString()));
        }
    }
}
