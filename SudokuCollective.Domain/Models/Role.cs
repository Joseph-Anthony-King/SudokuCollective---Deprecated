using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Domain.Models
{
    public class Role : IRole
    {
        #region Properties
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public RoleLevel RoleLevel { get; set; }
        public virtual List<IUserRole> Users { get; set; }
        #endregion

        #region Constructors
        public Role()
        {
            Id = 0;
            Name = string.Empty;
            RoleLevel = RoleLevel.NULL;
        }

        [JsonConstructor]
        public Role(int id, string name, RoleLevel roleLevel)
        {
            Id = id;
            Name = name;
            RoleLevel = roleLevel;
        }
        #endregion

        #region Methods
        public Role Convert(IRole role)
        {
            return (Role)role;
        }
        #endregion
    }
}
