namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IAppAdminRequest : IBaseRequest
    {
        string TargetLicense { get; set; }
    }
}
