using System;
using System.Collections.Generic;

namespace SudokuCollective.Domain.Interfaces { 

    public interface ISudokuCell {
        
        int Index { get; set; }
        int Column { get; set; }
        int Region { get; set; }
        int Row { get; set; }
        int Value { get; set; }
        int DisplayValue { get; set; }
        bool Obscured { get; set; }
        List<int> AvailableValues { get; set; }
        int ToInt32();
        void UpdateAvailableValues(int i);
        void ResetAvailableValues(int i);
        event EventHandler<UpdateSudokuCellEventArgs> SudokuCellUpdatedEvent;
        event EventHandler<ResetSudokuCellEventArgs> SudokuCellResetEvent;
        void OnSuccessfulSudokuCellUpdate(UpdateSudokuCellEventArgs e);
        void OnSuccessfulSudokuCellReset(ResetSudokuCellEventArgs e);
    }
}
