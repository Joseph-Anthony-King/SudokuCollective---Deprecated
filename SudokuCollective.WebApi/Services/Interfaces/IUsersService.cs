using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestObjects.RegisterRequests;
using SudokuCollective.WebApi.Models.RequestObjects.UserRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {
    
    public interface IUsersService {

        Task<User> GetUser(int id, bool fullRecord = true);
        Task<ActionResult<IEnumerable<User>>> GetUsers(bool fullRecord = true);
        Task<User> CreateUser(RegisterRO registerRO);
        Task<User> UpdateUser(int id, UpdateUserRO updateUserRO);
        Task<bool> UpdatePassword(int id, UpdatePasswordRO updatePasswordRO);
        Task<User> DeleteUser(int id);
        Task AddUserRoles(int userId, List<int> roleIds);
        Task RemoveUserRoles(int userId, List<int> roleIds);
        bool IsUserValid(User user);
    }
}
