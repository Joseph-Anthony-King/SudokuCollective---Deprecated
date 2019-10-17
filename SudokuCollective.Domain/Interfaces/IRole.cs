using SudokuCollective.Domain.Enums;

namespace SudokuCollective.Domain.Interfaces {

    public interface IRole : IEntityBase {

        string Name { get; set; }
        RoleLevel RoleLevel { get; set; }
    }
}
