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
        int DisplayValue { get; set; }
        bool Obscured { get; set; }
        int SudokuMatrixId { get; set; }
        SudokuMatrix SudokuMatrix { get; set; }
        List<IAvailableValue> AvailableValues { get; set; }
        int ToInt32() => DisplayValue;
        string ToString() => DisplayValue.ToString();
        void UpdateAvailableValues(int i);
        void ResetAvailableValues(int i);
        void OnSuccessfulSudokuCellUpdate(SudokuCellEventArgs e);
        event EventHandler<SudokuCellEventArgs> SudokuCellEvent;
    }
}
