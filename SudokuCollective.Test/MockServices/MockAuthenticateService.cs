using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.DTOModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Data.Models.TokenModels;

namespace SudokuCollective.Test.MockServices
{
    public class MockAuthenticateService
    {
        internal Mock<IAuthenticateService> AuthenticateServiceSuccessfulRequest { get; set; }
        internal Mock<IAuthenticateService> AuthenticateServiceFailedRequest { get; set; }

        public MockAuthenticateService()
        {
            AuthenticateServiceSuccessfulRequest = new Mock<IAuthenticateService>();
            AuthenticateServiceFailedRequest = new Mock<IAuthenticateService>();

            AuthenticateServiceSuccessfulRequest.Setup(authenticateService =>
                authenticateService.IsAuthenticated(It.IsAny<TokenRequest>()))
                    .Returns(Task.FromResult(new AuthenticatedUserResult() 
                    {
                        IsSuccess = true,
                        Message = UsersMessages.UserFoundMessage,
                        Token = "token",
                        User = new AuthenticatedUser()
                    } as IAuthenticatedUserResult));

            AuthenticateServiceFailedRequest.Setup(authenticateService =>
                authenticateService.IsAuthenticated(It.IsAny<TokenRequest>()))
                    .Returns(Task.FromResult(new AuthenticatedUserResult()
                    {
                        IsSuccess = false,
                        Message = UsersMessages.UserNotFoundMessage
                    } as IAuthenticatedUserResult));
        }
    }
}
