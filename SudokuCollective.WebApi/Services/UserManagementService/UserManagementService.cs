using Microsoft.EntityFrameworkCore;
using SudokuCollective.WebApi.Models.DataModels;
using SudokuCollective.WebApi.Services.Interfaces;
using System.Threading.Tasks;

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
    }
}
