using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Test.MockServices
{
    public class MockUserManagementService
    {
        internal Mock<IUserManagementService> UserManagementServiceSuccssfulRequest { get; set; }
        internal Mock<IUserManagementService> UserManagementServiceFailedRequest { get; set; }
        internal Mock<IUserManagementService> UserManagementServiceUserNameFailedRequest { get; set; }
        internal Mock<IUserManagementService> UserManagementServicePasswordFailedRequest { get; set; }

        public MockUserManagementService()
        {
            UserManagementServiceSuccssfulRequest = new Mock<IUserManagementService>();
            UserManagementServiceFailedRequest = new Mock<IUserManagementService>();
            UserManagementServiceUserNameFailedRequest = new Mock<IUserManagementService>();
            UserManagementServicePasswordFailedRequest = new Mock<IUserManagementService>();

            UserManagementServiceSuccssfulRequest.Setup(userManagementService =>
                userManagementService.IsValidUser(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(true));

            UserManagementServiceSuccssfulRequest.Setup(userManagementService =>
                userManagementService.ConfirmUserName(It.IsAny<string>()))
                    .Returns(Task.FromResult(new AuthenticationResult() 
                    {
                        Success = true,
                        Message = UsersMessages.UserNameConfirmedMessage,
                        UserName = "TestSuperUser"
                    } as IAuthenticationResult));

            UserManagementServiceFailedRequest.Setup(userManagementService =>
                userManagementService.IsValidUser(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(false));

            UserManagementServiceFailedRequest.Setup(userManagementService =>
                userManagementService.ConfirmUserName(It.IsAny<string>()))
                    .Returns(Task.FromResult(new AuthenticationResult()
                    {
                        Success = false,
                        Message = UsersMessages.NoUserIsUsingThisEmailMessage
                    } as IAuthenticationResult));

            UserManagementServiceUserNameFailedRequest.Setup(userManagementService =>
                userManagementService.ConfirmAuthenticationIssue(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(UserAuthenticationErrorType.USERNAMEINVALID));

            UserManagementServicePasswordFailedRequest.Setup(userManagementService =>
                userManagementService.ConfirmAuthenticationIssue(It.IsAny<string>(), It.IsAny<string>()))
                    .Returns(Task.FromResult(UserAuthenticationErrorType.PASSWORDINVALID));
        }
    }
}
