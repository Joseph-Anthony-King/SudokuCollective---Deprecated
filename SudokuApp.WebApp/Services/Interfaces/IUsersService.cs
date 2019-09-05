using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.RequestObjects;

namespace SudokuApp.WebApp.Services.Interfaces {
    
    public interface IUsersService {

        Task<ActionResult<User>> GetUser(int id);
        Task<ActionResult<IEnumerable<User>>> GetUsers();
        Task<User> CreateUser(UserRO userRO);
        Task UpdateUser(int id, User user);
        Task<User> DeleteUser(int id);
    }
}
