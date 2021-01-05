using System;
using System.Collections.Generic;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Structs;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface ISudokuCell : IEntityBase
    {
        int Index { get; set; }
        int Column { get; set; }
        int Region { get; set; }
        int Row { get; set; }
        int Value { get; set; }
        int DisplayedValue { get; set; }
        bool Hidden { get; set; }
        int SudokuMatrixId { get; set; }
        SudokuMatrix SudokuMatrix { get; set; }
        List<IAvailableValue> AvailableValues { get; set; }
        int ToInt32() => DisplayedValue;
        string ToString() => DisplayedValue.ToString();
        void UpdateAvailableValues(int i);
        void OnSuccessfulSudokuCellUpdate(SudokuCellEventArgs e);
        event EventHandler<SudokuCellEventArgs> SudokuCellEvent;
    }
}
