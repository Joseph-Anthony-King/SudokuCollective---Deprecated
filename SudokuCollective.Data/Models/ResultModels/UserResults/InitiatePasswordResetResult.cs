using SudokuCollective.Core.Interfaces.APIModels.ResultModels.UserResults;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class InitiatePasswordResetResult : IInitiatePasswordResetResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string Token { get; set; }
        public bool? ConfirmationEmailSuccessfullySent { get; set; }
        public IUser User { get; set; }
        public IApp App { get; set; }

        public InitiatePasswordResetResult() : base()
        {
            Token = null;
            ConfirmationEmailSuccessfullySent = null;
            User = new User();
            App = new App();
        }
    }
}
