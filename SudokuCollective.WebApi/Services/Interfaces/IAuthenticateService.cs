using SudokuCollective.WebApi.Models.TokenModels;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IAuthenticateService {

        bool IsAuthenticated(TokenRequest request, out string token);
    }
}
