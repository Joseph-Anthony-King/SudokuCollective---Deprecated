using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class AppUserRequest : IAppUserRequest
    {
        public string TargetLicense { get; set; }
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPageListModel PageListModel { get; set; }

        public AppUserRequest() : base()
        {
            TargetLicense = string.Empty;
            License = string.Empty;
            RequestorId = 0;
            AppId = 0;

            if (PageListModel == null)
            {
                PageListModel = new PageListModel();
            }
        }
    }
}
