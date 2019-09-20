using System.Threading.Tasks;

namespace SudokuApp.WebApi.Services.Interfaces {

    public interface IUserManagementService {

        Task<bool> IsValidUser(string email, string password);
    }
}
