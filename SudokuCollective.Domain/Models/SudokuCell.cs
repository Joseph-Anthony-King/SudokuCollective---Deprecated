﻿using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Structs;

namespace SudokuCollective.Domain.Models
{
    public class SudokuCell : ISudokuCell
    {
        #region Fields
        private int _value;
        private int _displayValue;
        private bool _initializing = true;
        #endregion

        #region Properties
        public int Id { get; set; }
        public int Index { get; set; }
        public int Column { get; set; }
        public int Region { get; set; }
        public int Row { get; set; }
        public int Value
        {
            get => _value;

            set
            {
                if (value == 0)
                {
                    if (!_initializing)
                    {
                        OnSuccessfulSudokuCellReset(
                            new ResetSudokuCellEventArgs(
                                Index,
                                _value,
                                Column,
                                Region,
                                Row
                            )
                        );

                        _value = value;
                    }
                }
                else
                {
                    AvailableValues = new List<int>();
                    _value = value;

                    OnSuccessfulSudokuCellUpdate(
                        new UpdateSudokuCellEventArgs(
                            Index,
                            _value,
                            Column,
                            Region,
                            Row
                        )
                    );
                }
            }
        }
        public int DisplayValue
        {
            get
            {
                if (!Obscured)
                {
                    return _value;
                }
                else
                {
                    return _displayValue;
                }
            }
            set
            {
                _displayValue = value;
            }
        }
        public bool Obscured { get; set; }
        public int SudokuMatrixId { get; set; }
        public ISudokuMatrix SudokuMatrix { get; set; }
        [JsonIgnore]
        public List<int> AvailableValues { get; set; }
        #endregion

        #region Constructors
        public SudokuCell(int index, int column, int region, int row, int matrixID)
        {
            Id = 0;

            AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            Index = index;
            Column = column;
            Region = region;
            Row = row;
            SudokuMatrixId = matrixID;
            Value = 0;
            Obscured = true;
            _initializing = false;
        }

        public SudokuCell(int index, int column, int region, int row, int value, int matrixId)
        {
            Id = 0;

            if (Value == 0)
            {

                AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            }
            else
            {

                AvailableValues = new List<int>();
            }

            Index = index;
            Column = column;
            Region = region;
            Row = row;
            SudokuMatrixId = matrixId;
            Value = value;
            Obscured = true;
            _initializing = false;
        }

        public SudokuCell()
        {
            Id = 0;
            Obscured = true;
            _initializing = false;
        }

        [JsonConstructor]
        public SudokuCell(
            int id,
            int index,
            int column,
            int region,
            int row,
            int value,
            int displayValue,
            bool obscured,
            int sudokuMatrixId)
        {
            AvailableValues = new List<int>();

            Id = id;
            Index = index;
            Column = column;
            Region = region;
            Row = row;
            _value = value;
            DisplayValue = displayValue;
            SudokuMatrixId = sudokuMatrixId;

            Obscured = obscured;
        }
        #endregion

        #region Methods
        public int ToInt32() => DisplayValue;

        public override string ToString() => DisplayValue.ToString();

        public void UpdateAvailableValues(int i)
        {
            if (i == 0)
            {
                AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            }
            else
            {
                if (AvailableValues.Contains(i) && AvailableValues.Count > 1 && Value == 0)
                {
                    AvailableValues.Remove(i);
                }
                else if (AvailableValues.Count == 1 && Value == 0)
                {
                    Value = AvailableValues[0];
                    AvailableValues = new List<int>();
                }
                else
                {
                    // do nothing...
                }
            }
        }

        public void ResetAvailableValues(int i)
        {
            if (Value == 0 && !AvailableValues.Contains(i))
            {
                AvailableValues.Add(i);
                var tmp = AvailableValues.Distinct().ToList();
                tmp.Remove(0);
                tmp.Sort();

                AvailableValues = new List<int>();
                AvailableValues.AddRange(tmp);
            }
        }

        public SudokuCell Convert(ISudokuCell cell)
        {
            return (SudokuCell)cell;
        }
        #endregion

        #region Event Handlers
        public event EventHandler<UpdateSudokuCellEventArgs> SudokuCellUpdatedEvent;

        public event EventHandler<ResetSudokuCellEventArgs> SudokuCellResetEvent;

        public virtual void OnSuccessfulSudokuCellUpdate(UpdateSudokuCellEventArgs e)
        {
            SudokuCellUpdatedEvent.Invoke(this, e);
        }

        public virtual void OnSuccessfulSudokuCellReset(ResetSudokuCellEventArgs e)
        {
            SudokuCellResetEvent.Invoke(this, e);
        }
        #endregion
    }
}
