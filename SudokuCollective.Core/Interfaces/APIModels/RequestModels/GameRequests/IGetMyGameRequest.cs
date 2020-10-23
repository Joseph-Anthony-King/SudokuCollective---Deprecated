namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IGetMyGameRequest : IBaseRequest
    {
        int UserId { get; set; }
    }
}
