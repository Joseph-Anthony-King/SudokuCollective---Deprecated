using System.Collections.Generic;
using System.Threading.Tasks;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels.RegisterRequests;
using SudokuCollective.WebApi.Models.RequestModels.UserRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.UserRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface IUsersService {

        Task<UserResult> GetUser(int id, bool fullRecord = false);
        Task<UsersResult> GetUsers(PageListModel pageListModel, bool fullRecord = false);
        Task<UserResult> CreateUser(RegisterRequest registerRO, bool addAdmin = false);
        Task<UserResult> UpdateUser(int id, UpdateUserRequest updateUserRO);
        Task<BaseResult> UpdatePassword(int id, UpdatePasswordRequest updatePasswordRO);
        Task<BaseResult> DeleteUser(int id);
        Task<BaseResult> AddUserRoles(int id, List<int> roleIds);
        Task<BaseResult> RemoveUserRoles(int id, List<int> roleIds);
        Task<BaseResult> ActivateUser(int id);
        Task<BaseResult> DeactivateUser(int id);
    }
}
