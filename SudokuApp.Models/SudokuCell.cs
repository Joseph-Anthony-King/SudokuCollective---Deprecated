using System;
using System.Collections.Generic;
using System.Linq;
using SudokuApp.Models.Interfaces;

namespace SudokuApp.Models {

    public class SudokuCell : ISudokuCell {

        private int _value;
        private int _displayValue;
        private bool _initializing = true;

        #region Properties
        public int Id { get; set; }
        public SudokuMatrix SudokuMatrix { get; set; }
        public int Index { get; set; }
        public int Column { get; set; }
        public int Region { get; set; }
        public int Row { get; set; }
        public int Value {

            get => _value;
            set {

                if (value == 0) {

                    if (!this._initializing) {

                        OnSuccessfulSudokuCellReset(
                            new ResetSudokuCellEventArgs(
                                this.Index,
                                this._value,
                                this.Column,
                                this.Region,
                                this.Row
                            )
                        );

                        this._value = value;
                    }

                } else {

                    this.AvailableValues = new List<int>();
                    this._value = value;

                    OnSuccessfulSudokuCellUpdate(
                        new UpdateSudokuCellEventArgs(
                            this.Index,
                            this._value,
                            this.Column,
                            this.Region,
                            this.Row
                        )
                    );
                }
            }
        }
        public int DisplayValue {

            get {
                
                if (!this.Obscured) {

                    return _value;

                } else {

                    return _displayValue;
                }
            }
            set {

                _displayValue = value;
            }
        }
        public bool Obscured { get; set; }
        public List<int> AvailableValues { get; set; }
        #endregion

        #region Constructors
        public SudokuCell(int index, int column, int region, int row) {

            this.AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };

            this.Index = index;
            this.Column = column;
            this.Region = region;
            this.Row = row;

            this.Value = 0;
            this.Obscured = true;

            this._initializing = false;
        }

        public SudokuCell(int index, int column, int region, int row, int value) {

            if (this.Value == 0) {

                this.AvailableValues = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
                
            } else {

                this.AvailableValues = new List<int>();
            }

            this.Index = index;
            this.Column = column;
            this.Region = region;
            this.Row = row;

            this.Value = value;
            this.Obscured = true;

            this._initializing = false;
        }
        #endregion

        public int ToInt32() => this.DisplayValue;

        public override string ToString() => this.DisplayValue.ToString();

        public void UpdateAvailableValues(int i) {

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

        public void ResetAvailableValues(int i) {

            if (this.Value == 0 && !this.AvailableValues.Contains(i)) {

                this.AvailableValues.Add(i);
                var tmp = this.AvailableValues.Distinct().ToList();
                tmp.Remove(0);
                tmp.Sort();

                this.AvailableValues = new List<int>();
                this.AvailableValues.AddRange(tmp);
            }
        }

        public event EventHandler<UpdateSudokuCellEventArgs> SudokuCellUpdatedEvent;

        public event EventHandler<ResetSudokuCellEventArgs> SudokuCellResetEvent;

        public virtual void OnSuccessfulSudokuCellUpdate(UpdateSudokuCellEventArgs e) {

            SudokuCellUpdatedEvent.Invoke(this, e);
        }

        public virtual void OnSuccessfulSudokuCellReset(ResetSudokuCellEventArgs e) {

            SudokuCellResetEvent.Invoke(this, e);
        }
    }

    public struct UpdateSudokuCellEventArgs {

        public int Index { get; set; }
        public int Value { get; set; }
        public int Column { get; set; }
        public int Region { get; set; }
        public int Row { get; set; }

        public UpdateSudokuCellEventArgs(int index, int value, int column, int region, int row) {

            Index = index;
            Value = value;
            Column = column;
            Region = region;
            Row = row;
        }
    }

    public struct ResetSudokuCellEventArgs {

        public int Index { get; set; }
        public int Value { get; set; }
        public int Column { get; set; }
        public int Region { get; set; }
        public int Row { get; set; }
        public List<int> Values { get; set; }

        public ResetSudokuCellEventArgs(int index, int value, int column, int region, int row) {

            Index = index;
            Value = value;
            Column = column;
            Region = region;
            Row = row;
            Values = new List<int>();
        }
    }
}
