using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.DTO;

namespace SudokuApp.WebApp.Services.Interfaces {
    
    public interface IUserService {

        Task<ActionResult<User>> GetUser(int userId);
        Task<ActionResult<IEnumerable<User>>> GetUsers();
        Task<User> CreateUser(UserDTO userDTO);
        Task UpdateUser(int userId, User user);
        Task<User> DeleteUser(int userId);
    }
}
