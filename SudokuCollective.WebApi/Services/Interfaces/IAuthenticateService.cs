using SudokuCollective.WebApi.Models.TokenModels;
using SudokuCollective.WebApi.Models.DTOModels;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IAuthenticateService {

        bool IsAuthenticated(TokenRequest request, out string token, out AuthenticatedUser user);
    }
}
