namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IAvailableValue
    {
        int Value { get; set; }
        bool Available { get; set; }
    }
}
