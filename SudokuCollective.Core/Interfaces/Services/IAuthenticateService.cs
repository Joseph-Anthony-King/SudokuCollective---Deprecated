using SudokuCollective.Core.Interfaces.APIModels.DTOModels;
using SudokuCollective.Core.Interfaces.APIModels.TokenModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IAuthenticateService
    {
        bool IsAuthenticated(ITokenRequest request, out string token, out IAuthenticatedUser user);
    }
}
