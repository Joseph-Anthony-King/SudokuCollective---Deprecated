using System;
using System.Collections.Generic;

namespace SudokuApp.Models {

    public class SudokuCell {

        private int _value;
        private int _availableValuesIndex;
        private bool _initializing = true;

        #region Properties
        public int Id { get; set; }
        public int Index { get; set; }
        public int Column { get; set; }
        public int Region { get; set; }
        public int Row { get; set; }
        public int Value {

            get => _value;
            set {

                if (value == 0) {

                    if (!_initializing) {

                        OnSuccessfulSudokuCellReset(
                            new ResetSudokuCellEventArgs(
                                this.Index,
                                this.AvailableValues,
                                this.Value,
                                this.Column,
                                this.Region,
                                this.Row
                            )
                        );
                    }

                } else {

                    this.AvailableValues = new List<int>();
                    this.AvailableValueIndex = 0;

                    OnSuccessfulSudokuCellUpdate(
                        new UpdateSudokuCellEventArgs(
                            this.Index,
                            value,
                            this.Column,
                            this.Region,
                            this.Row
                        )
                    );
                }

                this._value = value;
            }
        }
        public int AvailableValueIndex {

            get => this._availableValuesIndex;
            set {

                if (value >= AvailableValues.Count) {

                    this._availableValuesIndex = 0;

                } else {

                    this._availableValuesIndex = value;
                }
            }
        }
        public int DisplayValue { get => Obscured ? 0 : _value; }
        public bool Obscured { get; set; }
        public List<int> AvailableValues;
        #endregion

        #region Constructors
        public SudokuCell(int index, int column, int region, int row) {

            this.AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            this.Index = index;
            this.Column = column;
            this.Region = region;
            this.Row = row;

            this.Value = 0;
            this.AvailableValueIndex = 0;
            this.Obscured = true;

            _initializing = false;
        }

        public SudokuCell(int index, int column, int region, int row, int value) {

            if (this.Value == 0) {

                this.AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
            }

            this.Index = index;
            this.Column = column;
            this.Region = region;
            this.Row = row;

            this.Value = value;
            this.AvailableValueIndex = 0;
            this.Obscured = true;

            _initializing = false;
        }
        #endregion

        public int ToInt32() => DisplayValue;

        public override string ToString() => DisplayValue.ToString();

        internal void UpdateAvailableValues(int i) {

            if (i == 0) {

                this.AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                this.AvailableValueIndex = 0;

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

        internal event EventHandler<ResetSudokuCellEventArgs> SudokuCellResetEvent;

        internal virtual void OnSuccessfulSudokuCellUpdate(UpdateSudokuCellEventArgs e) {

            SudokuCellUpdatedEvent.Invoke(this, e);
        }

        internal virtual void OnSuccessfulSudokuCellReset(ResetSudokuCellEventArgs e) {

            SudokuCellResetEvent.Invoke(this, e);
            this.AvailableValues = e.Values;
        }
    }

    internal struct UpdateSudokuCellEventArgs {

        internal int Index { get; set; }
        internal int Value { get; set; }
        internal int Column { get; set; }
        internal int Region { get; set; }
        internal int Row { get; set; }

        internal UpdateSudokuCellEventArgs(int index, int value, int column, int region, int row) {

            Index = index;
            Value = value;
            Column = column;
            Region = region;
            Row = row;
        }
    }

    internal struct ResetSudokuCellEventArgs {

        internal int Index { get; set; }
        internal List<int> Values { get; set; }
        internal int Value { get; set; }
        internal int Column { get; set; }
        internal int Region { get; set; }
        internal int Row { get; set; }

        internal ResetSudokuCellEventArgs(int index, List<int> values, int value, int column, int region, int row) {

            Index = index;
            Values = values;
            Value = value;
            Column = column;
            Region = region;
            Row = row;
        }
    }
}
