using System.Threading.Tasks;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IUserManagementService {

        Task<bool> IsValidUser(string userName, string password);
    }
}
