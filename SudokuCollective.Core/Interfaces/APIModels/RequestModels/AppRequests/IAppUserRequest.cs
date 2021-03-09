namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IAppUserRequest : IBaseRequest
    {
        string TargetLicense { get; set; }
    }
}
