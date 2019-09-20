using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestObjects.RegisterRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {
    
    public interface IUsersService {

        Task<ActionResult<User>> GetUser(int id, bool fullRecord = true);
        Task<ActionResult<IEnumerable<User>>> GetUsers(bool fullRecord = true);
        Task<User> CreateUser(RegisterRO registerRO);
        Task UpdateUser(int id, User user);
        Task<User> DeleteUser(int id);
        Task AddUserRoles(int userId, List<int> roleIds);
        Task RemoveUserRoles(int userId, List<int> roleIds);
    }
}
