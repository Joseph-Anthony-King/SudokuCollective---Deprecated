using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Structs;

namespace SudokuCollective.Core.Models
{
    public class SudokuCell : ISudokuCell
    {
        #region Fields
        private int _value;
        private int _displayValue;
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
                    if (Value != 0)
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
                    }

                    _value = value;
                }
                else
                {
                    foreach (var availableValue in AvailableValues)
                    {
                        availableValue.Available = false;
                    }

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
        public SudokuMatrix SudokuMatrix { get; set; }
        [JsonIgnore]
        public List<IAvailableValue> AvailableValues { get; set; }
        #endregion

        #region Constructors
        public SudokuCell(
            int index, 
            int column, 
            int region, 
            int row, 
            int matrixID) : this()
        {
            Index = index;
            Column = column;
            Region = region;
            Row = row;
            SudokuMatrixId = matrixID;
            Value = 0;
        }

        public SudokuCell(
            int index, 
            int column, 
            int region, 
            int row, 
            int value, 
            int matrixId) : this()
        {
            if (Value != 0)
            {
                foreach (var availableValue in AvailableValues)
                {
                    availableValue.Available = false;
                }
            }

            Index = index;
            Column = column;
            Region = region;
            Row = row;
            SudokuMatrixId = matrixId;
            Value = value;
        }

        public SudokuCell()
        {
            Id = 0;
            Obscured = true;
            AvailableValues = new List<IAvailableValue>();

            for (var i = 1; i <= 9; i++)
            {
                AvailableValues.Add(
                        new AvailableValue
                        {
                            Value = i,
                            Errors = 0,
                            Available = true
                        }
                    );
            }
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
            Id = id;
            Index = index;
            Column = column;
            Region = region;
            Row = row;
            _value = value;
            DisplayValue = displayValue;
            Obscured = obscured;
            SudokuMatrixId = sudokuMatrixId;
            AvailableValues = new List<IAvailableValue>();

            var availability = true;

            if (value > 0)
            {
                availability = false;
            }

            for (var i = 1; i <= 9; i++)
            {
                AvailableValues.Add(
                        new AvailableValue
                        {
                            Value = i,
                            Errors = 0,
                            Available = availability
                        }
                    );
            }
        }
        #endregion

        #region Methods
        public int ToInt32() => DisplayValue;

        public override string ToString() => DisplayValue.ToString();

        public void UpdateAvailableValues(int i)
        {
            if (AvailableValues.Any(a => a.Value == i && a.Available) &&
                AvailableValues.Where(a => a.Available).ToList().Count > 1 &&
                Value == 0)
            {
                var availableValue = AvailableValues
                    .Where(a => a.Value == i)
                    .FirstOrDefault();

                availableValue.Available = false;
            }
            else if (AvailableValues.Where(a => a.Available).ToList().Count == 1 && Value == 0)
            {
                var availableValue = AvailableValues
                    .Where(a => a.Available)
                    .FirstOrDefault();

                Value = availableValue.Value;

                availableValue.Available = false;
            }
            else
            {
                // do nothing...
            }
        }

        public void ResetAvailableValues(int i)
        {
            if (Value == 0 && AvailableValues.Where(a => a.Value == i).Select(a => a.Available).FirstOrDefault())
            {
                var availableValue = AvailableValues
                    .Where(a => a.Value == i)
                    .FirstOrDefault();

                availableValue.Available = true;
            }
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
