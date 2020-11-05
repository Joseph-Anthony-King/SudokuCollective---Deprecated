using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.APIModels.TokenModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IAuthenticateService
    {
        Task<IAuthenticatedUserResult> IsAuthenticated(ITokenRequest request);
    }
}
