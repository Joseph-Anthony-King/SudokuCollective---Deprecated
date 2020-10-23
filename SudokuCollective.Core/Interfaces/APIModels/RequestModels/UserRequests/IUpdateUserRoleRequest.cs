using System.Collections.Generic;

namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IUpdateUserRoleRequest : IBaseRequest
    {
        List<int> RoleIds { get; set; }
    }
}
