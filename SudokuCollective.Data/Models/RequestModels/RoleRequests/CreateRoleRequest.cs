using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class CreateRoleRequest : ICreateRoleRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPageListModel PageListModel { get; set; }
        public string Name { get; set; }
        public RoleLevel RoleLevel { get; set; }

        public CreateRoleRequest() : base()
        {
            Name = string.Empty;
            RoleLevel = RoleLevel.NULL;
        }
    }
}
