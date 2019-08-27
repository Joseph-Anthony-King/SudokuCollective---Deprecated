using System.Threading.Tasks;

namespace SudokuApp.WebApp.Services.Interfaces {

    public interface IUserManagementService {

        Task<bool> IsValidUser(string email, string password);
    }
}
