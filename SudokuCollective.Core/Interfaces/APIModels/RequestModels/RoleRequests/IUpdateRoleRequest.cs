using SudokuCollective.Core.Enums;

namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface IUpdateRoleRequest : IBaseRequest
    {
        int Id { get; set; }
        string Name { get; set; }
        RoleLevel RoleLevel { get; set; }
    }
}
