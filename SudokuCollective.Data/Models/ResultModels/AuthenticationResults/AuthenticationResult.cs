using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class AuthenticationResult : IAuthenticationResult
    {
        public bool Success { get; set; }
        public bool FromCache { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }

        public AuthenticationResult() : base()
        {
            Success = false;
            FromCache = false;
            Message = string.Empty;
            UserName = string.Empty;
        }
    }
}
