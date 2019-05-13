using System;
using System.Collections.Generic;

namespace SudokuApp.Models {

    public class SudokuCell {

        private int _value;

        #region Properties
        public int Id { get; set; }
        public int Index { get; set; }
        public int Column { get; set; }
        public int Region { get; set; }
        public int Row { get; set; }
        public int Value {

            get => _value;
            set {

                this._value = value;

                if (this._value != 0) {

                    this.AvailableValues = new List<int>();

                    OnSuccessfulSudokuCellUpdate(
                        new UpdateSudokuCellEventArgs(
                            this.Value,
                            this.Column,
                            this.Region,
                            this.Row
                        )
                    );

                } else {

                    this.AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                }
            }
        }
        public int DisplayValue { get => Obscured ? 0 : _value; }
        public bool Obscured { get; set; }
        public List<int> AvailableValues;
        #endregion

        #region Constructors
        public SudokuCell(int index, int column, int region, int row) {

            this.Index = index;
            this.Column = column;
            this.Region = region;
            this.Row = row;

            this.Value = 0;
            this.Obscured = true;

            this.AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        }

        public SudokuCell(int index, int column, int region, int row, int value) {

            this.Index = index;
            this.Column = column;
            this.Region = region;
            this.Row = row;

            this.Value = value;
            this.Obscured = true;

            if (this.Value == 0) {

                this.AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }
        }
        #endregion

        public int ToInt32() => DisplayValue;

        public override string ToString() => DisplayValue.ToString();

        internal void UpdateAvailableValues(int i) {

            if (i == 0) {

                this.AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            } else {

                if (this.AvailableValues.Contains(i) && this.AvailableValues.Count > 0) {

                    this.AvailableValues.Remove(i);
                }

                if (this.AvailableValues.Count == 1) {

                    this.Value = this.AvailableValues[0];
                }
            }
        }

        internal event EventHandler<UpdateSudokuCellEventArgs> SudokuCellUpdatedEvent;

        internal virtual void OnSuccessfulSudokuCellUpdate(UpdateSudokuCellEventArgs e) {

            SudokuCellUpdatedEvent.Invoke(this, e);
        }
    }

    internal struct UpdateSudokuCellEventArgs {

        internal int Value { get; set; }
        internal int Column { get; set; }
        internal int Region { get; set; }
        internal int Row { get; set; }

        internal UpdateSudokuCellEventArgs(int value, int column, int region, int row) {

            Value = value;
            Column = column;
            Region = region;
            Row = row;
        }
    }
}
