using SudokuApp.WebApp.Models;

namespace SudokuApp.WebApp.Services.Interfaces {

    public interface IAuthenticateService {

        bool IsAuthenticated(TokenRequest request, out string token);
    }
}
