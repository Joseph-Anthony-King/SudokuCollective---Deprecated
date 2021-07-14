namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IGamesRequest : IBaseRequest
    {
        int UserId { get; set; }
    }
}
