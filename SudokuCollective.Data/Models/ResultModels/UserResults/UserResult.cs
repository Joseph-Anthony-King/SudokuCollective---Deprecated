using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class UserResult : IUserResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IUser User { get; set; }

        public UserResult() : base()
        {
            User = new User();
        }
    }
}
