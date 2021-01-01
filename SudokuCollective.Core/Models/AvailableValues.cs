using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Models
{
    public class AvailableValue : IAvailableValue
    {
        public int Value { get; set; }
        public int Errors { get; set; }
        public bool Available { get; set; }
    }
}
