using SudokuCollective.Core.Interfaces.APIModels.DTOModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Data.Models.DTOModels;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class AuthenticatedUserResult : IAuthenticatedUserResult
    {
        public bool IsSuccess { get; set; }
        public bool FromCache { get; set; }
        public string Message { get; set; }
        public IAuthenticatedUser User { get; set; }
        public string Token { get; set; }

        public AuthenticatedUserResult() : base()
        {
            IsSuccess = false;
            FromCache = false;
            Message = string.Empty;
            User = new AuthenticatedUser();
            Token = string.Empty;
        }
    }
}
