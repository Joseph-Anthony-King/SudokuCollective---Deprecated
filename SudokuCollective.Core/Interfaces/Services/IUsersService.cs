using System.Collections.Generic;
using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels.UserResults;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface IUsersService : IService
    {
        Task<IUserResult> GetUser(int id, bool fullRecord = true);
        Task<IUsersResult> GetUsers(IPageListModel pageListModel, bool fullRecord = true);
        Task<IUserResult> CreateUser(
            IRegisterRequest request,
            string baseUrl,
            string emailtTemplatePath);
        Task<IUserResult> UpdateUser(
            int id, 
            IUpdateUserRequest request,
            string baseUrl,
            string emailtTemplatePath);
        Task<IBaseResult> RequestPasswordReset(
            IRequestPasswordResetRequest request,
            string baseUrl,
            string emailtTemplatePath);
        Task<IInitiatePasswordResetResult> InitiatePasswordReset(string token);
        Task<IBaseResult> UpdatePassword(IPasswordResetRequest request);
        Task<IBaseResult> DeleteUser(int id);
        Task<IBaseResult> AddUserRoles(int userid, List<int> roleIds);
        Task<IBaseResult> RemoveUserRoles(int userid, List<int> roleIds);
        Task<IBaseResult> ActivateUser(int id);
        Task<IBaseResult> DeactivateUser(int id);
        Task<IConfirmEmailResult> ConfirmEmail(
            string token,
            string baseUrl,
            string emailtTemplatePath);
    }
}
