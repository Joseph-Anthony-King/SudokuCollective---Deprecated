using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class AppRequest : IAppRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPageListModel PageListModel { get; set; }
        public string Name { get; set; }
        public string DevUrl { get; set; }
        public string LiveUrl { get; set; }
        public bool IsActive { get; set; }
        public bool InDevelopment { get; set; }
        public bool PermitSuperUserAccess { get; set; }
        public bool PermitCollectiveLogins { get; set; }
        public bool DisableCustomUrls { get; set; }
        public string CustomEmailConfirmationDevUrl { get; set; }
        public string CustomEmailConfirmationLiveUrl { get; set; }
        public string CustomPasswordResetDevUrl { get; set; }
        public string CustomPasswordResetLiveUrl { get; set; }

        public AppRequest() : base()
        {
            Name = string.Empty;
            DevUrl = string.Empty;
            LiveUrl = string.Empty;
            IsActive = false;
            InDevelopment = false;
            PermitSuperUserAccess = false;
            PermitCollectiveLogins = false;
            DisableCustomUrls = false;
            CustomEmailConfirmationDevUrl = string.Empty;
            CustomEmailConfirmationLiveUrl = string.Empty;
            CustomPasswordResetDevUrl = string.Empty;
            CustomPasswordResetLiveUrl = string.Empty;

            if (PageListModel == null)
            {
                PageListModel = new PageListModel();
            }
        }
    }
}
