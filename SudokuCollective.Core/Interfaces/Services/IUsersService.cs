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
        Task<IUserResult> Create(
            IRegisterRequest request,
            string baseUrl,
            string emailtTemplatePath);
        Task<IUserResult> Get(
            int id,
            string license);
        Task<IUsersResult> GetUsers(
            int requestorId,
            string license,
            IPaginator paginator);
        Task<IUserResult> Update(
            int id, 
            IUpdateUserRequest request,
            string baseUrl,
            string emailTemplatePath);
        Task<IBaseResult> Delete(int id);
        Task<IBaseResult> RequestPasswordReset(
            IRequestPasswordResetRequest request,
            string baseUrl,
            string emailTemplatePath);
        Task<IInitiatePasswordResetResult> InitiatePasswordReset(string token);
        Task<IBaseResult> UpdatePassword(IPasswordResetRequest request);
        Task<IRolesResult> AddUserRoles(
            int userid,
            List<int> roleIds,
            string license);
        Task<IBaseResult> RemoveUserRoles(
            int userid,
            List<int> roleIds,
            string license);
        Task<IBaseResult> Activate(int id);
        Task<IBaseResult> Deactivate(int id);
        Task<IConfirmEmailResult> ConfirmEmail(
            string token,
            string baseUrl,
            string emailtTemplatePath);
        Task<IBaseResult> ResendPasswordReset(
            int userId,
            int appId,
            string baseUrl, 
            string emailTamplatePath);
        Task<IUserResult> ResendEmailConfirmation(
            int userId, 
            int appId,
            string baseUrl,
            string emailTemplatePath);
        Task<IUserResult> CancelEmailConfirmationRequest(int id, int appId);
        Task<IUserResult> CancelPasswordResetRequest(int id, int appId);
        Task<IUserResult> CancelAllEmailRequests(int id, int appId);
    }
}
