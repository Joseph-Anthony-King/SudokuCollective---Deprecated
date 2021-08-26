using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class UserResult : IUserResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFromCache { get; set; }
        public string Message { get; set; }
        public IUser User { get; set; }
        public bool? ConfirmationEmailSuccessfullySent { get; set; }
        public string Token { get; set; }

        public UserResult() : base()
        {
            IsSuccess = false;
            IsFromCache = false;
            Message = string.Empty;
            User = new User();
            ConfirmationEmailSuccessfullySent = null;
            Token = null;

        }
    }
}
