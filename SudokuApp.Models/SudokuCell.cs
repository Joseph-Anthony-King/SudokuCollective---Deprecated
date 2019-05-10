using System;

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

                    this.AvailableValues = string.Empty;

                    OnSuccessfulSudokuCellUpdate(
                        new UpdateSudokuCellEventArgs(
                            this.Value,
                            this.Column,
                            this.Region,
                            this.Row
                        )
                    );

                } else {

                    this.AvailableValues = "123456789";
                }
            }
        }
        public int DisplayValue {

            get {

                if (Obscured) {

                    return 0;

                } else {

                    return _value;
                }
            }
        }
        public bool Obscured { get; set; }
        public string AvailableValues { get; set; }
        #endregion

        #region Constructors
        public SudokuCell(int index, int column, int region, int row) {

            this.Index = index;
            this.Column = column;
            this.Region = region;
            this.Row = row;

            this.Value = 0;
            this.Obscured = true;
        }

        public SudokuCell(int index, int column, int region, int row, int value) {

            this.Index = index;
            this.Column = column;
            this.Region = region;
            this.Row = row;

            this.Value = value;
            this.Obscured = true;
        }
        #endregion

        internal void updateAvailableValues(string s) {

            if (string.IsNullOrEmpty(s)) {

                this.AvailableValues = "123456789";

            } else {

                var index = 0;

                for (var i = 0; i < AvailableValues.Length; i++) {

                    if (AvailableValues[i].Equals(s)) {

                        index = 0;
                    }

                    this.AvailableValues = this.AvailableValues.Remove(index, 1);
                }
            }
        }

        public int ToInt32() => DisplayValue;

        public override string ToString() => DisplayValue.ToString();

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
