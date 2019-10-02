using System.Collections.Generic;
using System.Threading.Tasks;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels.RegisterRequests;
using SudokuCollective.WebApi.Models.RequestModels.UserRequests;
using SudokuCollective.WebApi.Models.TaskModels;
using SudokuCollective.WebApi.Models.TaskModels.UserRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IUsersService {

        Task<UserTaskResult> GetUser(int id, bool fullRecord = true);
        Task<UserListTaskResult> GetUsers(bool fullRecord = true);
        Task<UserTaskResult> CreateUser(RegisterRO registerRO);
        Task<UserTaskResult> UpdateUser(int id, UpdateUserRO updateUserRO);
        Task<BaseTaskResult> UpdatePassword(int id, UpdatePasswordRO updatePasswordRO);
        Task<BaseTaskResult> DeleteUser(int id);
        Task<BaseTaskResult> AddUserRoles(int id, List<int> roleIds);
        Task<BaseTaskResult> RemoveUserRoles(int id, List<int> roleIds);
        Task<BaseTaskResult> ActivateUser(int id);
        Task<BaseTaskResult> DeactivateUser(int id);
        bool IsUserValid(User user);
    }
}
