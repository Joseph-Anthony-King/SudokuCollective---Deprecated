using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class RolesResult : IRolesResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFromCache { get; set; }
        public string Message { get; set; }
        public List<IRole> Roles { get; set; }

        public RolesResult() : base()
        {
            IsSuccess = false;
            IsFromCache = false;
            Message = string.Empty;
            Roles = new List<IRole>();
        }
    }
}
