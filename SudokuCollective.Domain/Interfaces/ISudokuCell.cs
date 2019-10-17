using System;
using System.Collections.Generic;

namespace SudokuCollective.Domain.Interfaces { 

    public interface ISudokuCell : IEntityBase {
        
        int Index { get; set; }
        int Column { get; set; }
        int Region { get; set; }
        int Row { get; set; }
        int Value { get; set; }
        int DisplayValue { get; set; }
        bool Obscured { get; set; }
        int SudokuMatrixId { get; set; }
    }
}
