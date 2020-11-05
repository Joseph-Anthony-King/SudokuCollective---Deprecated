using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Data.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUsersRepository<User> usersRepository;
        private readonly string emailDoesNotExistMessage;

        public UserManagementService(IUsersRepository<User> usersRepo)
        {
            usersRepository = usersRepo;
            emailDoesNotExistMessage = "Email Does Not Exist";
        }

        public async Task<bool> IsValidUser(string username, string password)
        {
            var userResponse = await usersRepository.GetByUserName(username);

            if (userResponse.Success)
            {
                if ((IUser)userResponse.Object != null
                    && BCrypt.Net.BCrypt.Verify(password, ((IUser)userResponse.Object).Password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public async Task<UserAuthenticationErrorType> ConfirmAuthenticationIssue(string username, string password)
        {
            var userResponse = await usersRepository.GetByUserName(username);

            if (userResponse.Success)
            {
                if ((IUser)userResponse.Object == null)
                {
                    return UserAuthenticationErrorType.USERNAMEINVALID;
                }
                else if (!BCrypt.Net.BCrypt.Verify(password, ((IUser)userResponse.Object).Password))
                {
                    return UserAuthenticationErrorType.PASSWORDINVALID;
                }
                else
                {
                    return UserAuthenticationErrorType.NULL;
                }
            }
            else
            {
                return UserAuthenticationErrorType.NULL;
            }
        }

        public async Task<IAuthenticationResult> ConfirmUserName(string email)
        {
            var result = new AuthenticationResult();

            var userResponse = await usersRepository.GetByEmail(email);

            if (userResponse.Success)
            {
                result.Success = true;
                result.UserName = ((User)userResponse.Object).UserName;

                return result;
            }
            else
            {
                result.Success = false;
                result.Message = emailDoesNotExistMessage;

                return result;
            }
        }
    }
}
