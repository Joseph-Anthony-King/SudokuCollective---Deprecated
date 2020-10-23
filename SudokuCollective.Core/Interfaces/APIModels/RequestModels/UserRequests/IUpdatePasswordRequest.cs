namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IUpdatePasswordRequest : IBaseRequest
    {
        string OldPassword { get; set; }
        string NewPassword { get; set; }
    }
}
