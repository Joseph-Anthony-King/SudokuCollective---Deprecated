namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface ICreateGameRequest : IBaseRequest
    {
        int UserId { get; set; }
        int DifficultyId { get; set; }
    }
}
