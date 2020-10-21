using System.Threading.Tasks;
using SudokuCollective.WebApi.Models.Enums;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IUserManagementService {

        Task<bool> IsValidUser(string userName, string password);
        Task<UserAuthenticationErrorType> ConfirmAuthenticationIssue(string userName, string password);
    }
}
