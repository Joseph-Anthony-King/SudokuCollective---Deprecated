namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IAddSolutionRequest :IBaseRequest
    {
        int Limit { get; set; }
    }
}
