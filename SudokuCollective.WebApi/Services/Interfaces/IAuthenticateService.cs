using SudokuCollective.WebApi.Models;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IAuthenticateService {

        bool IsAuthenticated(TokenRequest request, out string token);
    }
}
