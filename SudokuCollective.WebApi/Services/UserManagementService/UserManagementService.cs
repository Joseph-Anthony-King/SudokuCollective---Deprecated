using Microsoft.EntityFrameworkCore;
using SudokuCollective.WebApi.Models.DataModels;
using SudokuCollective.WebApi.Models.Enums;
using SudokuCollective.WebApi.Models.ResultModels.AuthenticationResults;
using SudokuCollective.WebApi.Services.Interfaces;
using System.Threading.Tasks;
using System.Linq;

namespace SudokuCollective.WebApi.Services {

    public class UserManagementService : IUserManagementService {

        private readonly DatabaseContext _context;

        public UserManagementService(DatabaseContext context) {

            _context = context;
        }

        public async Task<bool> IsValidUser(string userName, string password) {

            var user = await _context.Users.SingleOrDefaultAsync(u => 
                u.UserName.Equals(userName));

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.Password)) {

                return true;

            } else {

                return false;
            }
        }

        public async Task<UserAuthenticationErrorType> ConfirmAuthenticationIssue(string userName, string password) {

            var user = await _context.Users.SingleOrDefaultAsync(u =>
                u.UserName.Equals(userName));

            if (user == null) {

                return UserAuthenticationErrorType.USERNAMEINVALID;

            } else if (!BCrypt.Net.BCrypt.Verify(password, user.Password)) {

                return UserAuthenticationErrorType.PASSWORDINVALID;

            } else {

                return UserAuthenticationErrorType.NULL;
            }
        }

        public async Task<AuthenticationResult> ConfirmUserName(string email) {

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
