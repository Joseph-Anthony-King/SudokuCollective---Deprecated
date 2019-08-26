using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.WebApp.Services.Interfaces;
using System.Linq;

namespace SudokuApp.WebApp.Services {

    public class UserManagementService : IUserManagementService {

        private readonly ApplicationDbContext _context;

        public UserManagementService(ApplicationDbContext context) {

            _context = context;
        }

        public bool IsValidUser(string userName, string password) {

            var user = _context.Users.Where(u => u.UserName == userName && u.Password == password).FirstOrDefault();

            if (user != null) {

                return true;

            } else {

                return false;
            }
        }
    }
}
