using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class AppAdminRequest : IAppAdminRequest
    {
        public string TargetLicense { get; set; }
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPageListModel PageListModel { get; set; }

        public AppAdminRequest() : base()
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
