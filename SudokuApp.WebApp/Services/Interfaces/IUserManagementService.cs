namespace SudokuApp.WebApp.Services.Interfaces {

    public interface IUserManagementService {

        bool IsValidUser(string email, string password);
    }
}
