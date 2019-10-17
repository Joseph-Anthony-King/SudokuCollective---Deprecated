namespace SudokuCollective.Domain.Interfaces {

    public interface ISudokuMatrix : IEntityBase {

        int DifficultyId { get; set; }
    }
}
