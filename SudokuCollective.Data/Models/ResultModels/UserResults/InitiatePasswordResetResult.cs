using SudokuCollective.Core.Interfaces.APIModels.ResultModels.UserResults;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class InitiatePasswordResetResult : IInitiatePasswordResetResult
    {
        public bool IsSuccess { get; set; }
        public bool FromCache { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public bool? ConfirmationEmailSuccessfullySent { get; set; }
        public IUser User { get; set; }
        public IApp App { get; set; }

        public InitiatePasswordResetResult() : base()
        {
            IsSuccess = false;
            FromCache = false;
            Message = string.Empty;
            Token = null;
            ConfirmationEmailSuccessfullySent = null;
            User = new User();
            App = new App();
        }
    }
}
