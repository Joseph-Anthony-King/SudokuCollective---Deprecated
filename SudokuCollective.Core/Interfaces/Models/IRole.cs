using System.Collections.Generic;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IRole : IEntityBase
    {
        string Name { get; set; }
        RoleLevel RoleLevel { get; set; }
        public List<UserRole> Users { get; set; }
    }
}
