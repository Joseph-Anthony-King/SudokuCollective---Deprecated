using System.Collections.Generic;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class RoleResult : IRoleResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IRole Role { get; set; }

        public RoleResult() : base()
        {
            Role = new Role()
            {
                Id = 0,
                Name = string.Empty,
                RoleLevel = RoleLevel.NULL,
                Users = new List<IUserRole>()
            };
        }
    }
}