using System.Collections.Generic;
using System.Threading.Tasks;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels.RegisterRequests;
using SudokuCollective.WebApi.Models.RequestModels.UserRequests;
using SudokuCollective.WebApi.Models.TaskModels.UserRequests;

namespace SudokuCollective.WebApi.Services.Interfaces
{

    public interface IUsersService {

        Task<UserTaskResult> GetUser(int id, bool fullRecord = true);
        Task<UserListTaskResult> GetUsers(bool fullRecord = true);
        Task<UserTaskResult> CreateUser(RegisterRO registerRO);
        Task<UserTaskResult> UpdateUser(int id, UpdateUserRO updateUserRO);
        Task<bool> UpdatePassword(int id, UpdatePasswordRO updatePasswordRO);
        Task<bool> DeleteUser(int id);
        Task<bool> AddUserRoles(int userId, List<int> roleIds);
        Task<bool> RemoveUserRoles(int userId, List<int> roleIds);
        bool IsUserValid(User user);
    }
}
