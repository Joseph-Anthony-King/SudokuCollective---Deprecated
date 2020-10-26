using System.Collections.Generic;

namespace SudokuCollective.Core.Structs
{
    public struct ResetSudokuCellEventArgs
    {
        public int Index { get; set; }
        public int Value { get; set; }
        public int Column { get; set; }
        public int Region { get; set; }
        public int Row { get; set; }
        public List<int> Values { get; set; }

        public ResetSudokuCellEventArgs(
            int index, 
            int value, 
            int column, 
            int region, 
            int row)
        {
            Index = index;
            Value = value;
            Column = column;
            Region = region;
            Row = row;
            Values = new List<int>();
        }
    }
}
