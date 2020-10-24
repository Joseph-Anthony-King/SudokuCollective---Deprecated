using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Models;
using System.Collections.Generic;

namespace SudokuCollective.Core.Interfaces.Models
{
    public interface IRole : IEntityBase
    {
        string Name { get; set; }
        RoleLevel RoleLevel { get; set; }
        public List<UserRole> Users { get; set; }
    }
}
