using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class RoleResult : IRoleResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFromCache { get; set; }
        public string Message { get; set; }
        public IRole Role { get; set; }

        public RoleResult() : base()
        {
            IsSuccess = false;
            IsFromCache = false;
            Message = string.Empty;
            Role = new Role();
        }
    }
}