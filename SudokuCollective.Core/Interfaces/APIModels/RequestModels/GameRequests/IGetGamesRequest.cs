namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IGetGamesRequest : IBaseRequest
    {
        int UserId { get; set; }
    }
}
