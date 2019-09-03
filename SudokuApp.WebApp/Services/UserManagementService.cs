using Microsoft.EntityFrameworkCore;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.WebApp.Services.Interfaces;
using System.Linq;
using System.Threading.Tasks;

namespace SudokuApp.WebApp.Services {

    public class UserManagementService : IUserManagementService {

        private readonly ApplicationDbContext _context;

        public UserManagementService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<bool> IsValidUser(string userName, string password) {

            var user = await _context.Users.SingleOrDefaultAsync(u => 
                u.UserName.Equals(userName) && 
                BCrypt.Net.BCrypt.Verify(password, u.Password));

            if (user != null) {

                return true;

            } else {

                return false;
            }
        }
    }
}
