namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IAppRequest : IBaseRequest
    {
        string Name { get; set; }
        string DevUrl { get; set; }
        string LiveUrl { get; set; }
        bool IsActive { get; set; }
        bool InDevelopment { get; set; }
        bool PermitSuperUserAccess { get; set; }
        bool PermitCollectiveLogins { get; set; }
        bool DisableCustomUrls { get; set; }
        string CustomEmailConfirmationDevUrl { get; set; }
        string CustomEmailConfirmationLiveUrl { get; set; }
        string CustomPasswordResetDevUrl { get; set; }
        string CustomPasswordResetLiveUrl { get; set; }
    }
}
