using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class RoleResult : IRoleResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IRole Role { get; set; }

        public RoleResult() : base()
        {
            Role = new Role();
        }
    }
}