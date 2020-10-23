using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class AuthenticationResult : IAuthenticationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }

        public AuthenticationResult() : base()
        {
            UserName = string.Empty;
        }
    }
}
