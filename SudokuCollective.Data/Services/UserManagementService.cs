using System.Threading.Tasks;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Data.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly IUsersRepository<User> usersRepository;

        public UserManagementService(IUsersRepository<User> usersRepo)
        {
            usersRepository = usersRepo;
        }

        public async Task<bool> IsValidUser(string username, string password)
        {
            var userResponse = await usersRepository.GetByUserName(username, true);

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
                result.Message = UsersMessages.UserNameConfirmedMessage;
                result.UserName = ((User)userResponse.Object).UserName;

                return result;
            }
            else
            {
                result.Success = false;
                result.Message = UsersMessages.EmailDoesNotExist;

                return result;
            }
        }
    }
}
