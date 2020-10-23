using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class UsersResult : IUsersResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<IUser> Users { get; set; }

        public UsersResult() : base()
        {
            Users = new List<IUser>();
        }
    }
}
