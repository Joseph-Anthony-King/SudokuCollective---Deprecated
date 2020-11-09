using System.Collections.Generic;
using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IUsersService
    {
        Task<IUserResult> GetUser(int id, bool fullRecord = false);
        Task<IUsersResult> GetUsers(IPageListModel pageListModel, bool fullRecord = false);
        Task<IUserResult> CreateUser(IRegisterRequest registerRequest);
        Task<IUserResult> UpdateUser(int id, IUpdateUserRequest updateUserRequest);
        Task<IBaseResult> UpdatePassword(int id, IUpdatePasswordRequest updatePasswordRequest);
        Task<IBaseResult> DeleteUser(int id);
        Task<IBaseResult> AddUserRoles(int userid, List<int> roleIds);
        Task<IBaseResult> RemoveUserRoles(int userid, List<int> roleIds);
        Task<IBaseResult> ActivateUser(int id);
        Task<IBaseResult> DeactivateUser(int id);
    }
}
