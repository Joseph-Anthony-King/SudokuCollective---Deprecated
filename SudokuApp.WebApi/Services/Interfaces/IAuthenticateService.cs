using SudokuApp.WebApi.Models;

namespace SudokuApp.WebApi.Services.Interfaces {

    public interface IAuthenticateService {

        bool IsAuthenticated(TokenRequest request, out string token);
    }
}
