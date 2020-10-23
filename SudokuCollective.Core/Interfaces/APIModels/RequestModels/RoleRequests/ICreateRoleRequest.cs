using SudokuCollective.Core.Enums;

namespace SudokuCollective.Core.Interfaces.APIModels.RequestModels
{
    public interface ICreateRoleRequest : IBaseRequest
    {
        string Name { get; set; }
        RoleLevel RoleLevel { get; set; }
    }
}
