using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdateRoleRequest : IUpdateRoleRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPaginator Paginator { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public RoleLevel RoleLevel { get; set; }

        public UpdateRoleRequest() : base()
        {
            Id = 0;
            Name = string.Empty;
            RoleLevel = RoleLevel.NULL;

            if (Paginator == null)
            {
                Paginator = new Paginator();
            }
        }
    }
}
