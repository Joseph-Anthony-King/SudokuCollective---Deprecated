using System.Threading.Tasks;
using SudokuCollective.WebApi.Models.Enums;
using SudokuCollective.WebApi.Models.ResultModels.AuthenticationResults;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IUserManagementService {

        Task<bool> IsValidUser(string userName, string password);
        Task<UserAuthenticationErrorType> ConfirmAuthenticationIssue(string userName, string password);
        Task<AuthenticationResult> ConfirmUserName(string email);
    }
}
