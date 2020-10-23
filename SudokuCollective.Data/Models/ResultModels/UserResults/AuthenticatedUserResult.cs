using SudokuCollective.Core.Interfaces.APIModels.DTOModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Data.Models.DTOModels;

namespace SudokuCollective.Data.Models.ResultModels
{

    public class AuthenticatedUserResult : IAuthenticatedUserResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public IAuthenticatedUser User { get; set; }
        public string Token { get; set; }

        public AuthenticatedUserResult() : base()
        {
            User = new AuthenticatedUser();
            Token = string.Empty;
        }
    }
}
