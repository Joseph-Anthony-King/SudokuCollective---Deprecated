using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Data.Services
{
    public class UserManagementService : IUserManagementService
    {
        private readonly DatabaseContext _context;

        public UserManagementService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<bool> IsValidUser(string userName, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u =>
                u.UserName.Equals(userName));

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<UserAuthenticationErrorType> ConfirmAuthenticationIssue(string userName, string password)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u =>
                u.UserName.Equals(userName));

            if (user == null)
            {
                return UserAuthenticationErrorType.USERNAMEINVALID;
            }
            else if (!BCrypt.Net.BCrypt.Verify(password, user.Password))
            {
                return UserAuthenticationErrorType.PASSWORDINVALID;
            }
            else
            {
                return UserAuthenticationErrorType.NULL;
            }
        }

        public async Task<IAuthenticationResult> ConfirmUserName(string email)
        {
            var user = await _context.Users.SingleOrDefaultAsync(u =>
                u.Email.Equals(email));

            var result = new AuthenticationResult();

            if (user != null)
            {
                result.Success = true;
                result.UserName = user.UserName;

                return result;
            }
            else
            {
                result.Success = false;
                result.Message = "Email Does Not Exist";

                return result;
            }
        }
    }
}
