using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdateUserRoleRequest : IUpdateUserRoleRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPageListModel PageListModel { get; set; }
        public List<int> RoleIds { get; set; }

        public UpdateUserRoleRequest() : base()
        {
            RoleIds = new List<int>();

            if (PageListModel == null)
            {
                PageListModel = new PageListModel();
            }
        }
    }
}
