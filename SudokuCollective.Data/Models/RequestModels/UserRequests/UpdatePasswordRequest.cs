using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdatePasswordRequest : IUpdatePasswordRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPageListModel PageListModel { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public UpdatePasswordRequest() : base()
        {
            OldPassword = string.Empty;
            NewPassword = string.Empty;

            if (PageListModel == null)
            {
                PageListModel = new PageListModel();
            }
        }
    }
}
