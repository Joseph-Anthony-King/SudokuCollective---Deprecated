using Microsoft.EntityFrameworkCore;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuCollective.WebApi.Services {

    public class UserManagementService : IUserManagementService {

        private readonly ApplicationDbContext _context;

        public UserManagementService(ApplicationDbContext context) {

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
