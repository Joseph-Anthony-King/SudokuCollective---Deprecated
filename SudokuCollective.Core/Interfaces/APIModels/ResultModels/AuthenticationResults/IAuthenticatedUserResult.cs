using SudokuCollective.Core.Interfaces.APIModels.DTOModels;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IAuthenticatedUserResult : IBaseResult
    {
       IAuthenticatedUser User { get; set; }
       string Token { get; set; }
    }
}
