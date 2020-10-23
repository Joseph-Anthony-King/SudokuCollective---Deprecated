using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IUserManagementService
    {
        Task<bool> IsValidUser(string userName, string password);
        Task<UserAuthenticationErrorType> ConfirmAuthenticationIssue(string userName, string password);
        Task<IAuthenticationResult> ConfirmUserName(string email);
    }
}
