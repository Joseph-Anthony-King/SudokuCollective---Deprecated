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

        public int ToInt32() => DisplayValue;

        public override string ToString() => DisplayValue.ToString();

        public string PrintValue() => string.Format("[{0}]", this.Value);

        public string PrintDisplayedValue() {

            if (this.Obscured) {

                return string.Format("[{0}]", this.DisplayValue);

            } else {

                return string.Format("[{0}]", this.Value);
            }
        }

        public string PrintCoordinates() {

            if (this.Row != 1) {

                return string.Format("[{0}:{1},{2},{3}]", this.Index, this.Column, this.Region, this.Row);
            } else {

                return string.Format("[ {0}:{1},{2},{3}]", this.Index, this.Column, this.Region, this.Row);
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
