using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels.UserResults;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Test.MockRepositories;

namespace SudokuCollective.Test.MockServices
{
    public class MockUsersService
    {
        private MockUsersRepository MockUsersRespository { get; set; }
        private MockEmailService MockEmailService { get; set; }

        internal Mock<IUsersService> UsersServiceSuccessfulRequest { get; set; }
        internal Mock<IUsersService> UsersServiceFailedRequest { get; set; }

        public MockUsersService(DatabaseContext context)
        {
            MockUsersRespository = new MockUsersRepository(context);

            UsersServiceSuccessfulRequest = new Mock<IUsersService>();
            UsersServiceFailedRequest = new Mock<IUsersService>();

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.CreateUser(It.IsAny<RegisterRequest>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Create(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserCreatedMessage,
                    User = (User)MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Create(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.GetUser(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserFoundMessage,
                    User = (User)MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.GetUsers(It.IsAny<PageListModel>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = UsersMessages.UsersFoundMessage,
                    Users = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Objects
                        .ConvertAll(u => (IUser)u)
                } as IUsersResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.UpdateUser(It.IsAny<int>(), It.IsAny<UpdateUserRequest>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserUpdatedMessage,
                    User = (User)MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.UpdatePassword(It.IsAny<int>(), It.IsAny<UpdatePasswordRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.PasswordUpdatedMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.DeleteUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Delete(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserDeletedMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.AddUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .AddRoles(It.IsAny<int>(), It.IsAny<List<int>>())
                        .Result
                        .Success,
                    Message = UsersMessages.RolesAddedMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.RemoveUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .RemoveRoles(It.IsAny<int>(), It.IsAny<List<int>>())
                        .Result
                        .Success,
                    Message = UsersMessages.RolesRemovedMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.ActivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .ActivateUser(It.IsAny<int>())
                        .Result,
                    Message = UsersMessages.UserActivatedMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.DeactivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .DeactivateUser(It.IsAny<int>())
                        .Result,
                    Message = UsersMessages.UserDeactivatedMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(usersService =>
                usersService.ConfirmEmail(It.IsAny<string>()))
                .Returns(Task.FromResult(new ConfirmEmailResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .ConfirmEmail(It.IsAny<string>())
                        .Result
                        .Success,
                    Message = UsersMessages.EmailConfirmedMessage
                } as IConfirmEmailResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.CreateUser(It.IsAny<RegisterRequest>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Create(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserNotCreatedMessage,
                    User = (User)MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Create(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.GetUser(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserNotFoundMessage,
                    User = (User)MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Object,
                } as IUserResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.GetUsers(It.IsAny<PageListModel>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = UsersMessages.UsersNotFoundMessage,
                    Users = null
                } as IUsersResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.UpdateUser(It.IsAny<int>(), It.IsAny<UpdateUserRequest>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserNotUpdatedMessage,
                    User = (User)MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.UpdatePassword(It.IsAny<int>(), It.IsAny<UpdatePasswordRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.PasswordNotUpdatedMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.DeleteUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Delete(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserNotDeletedMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.AddUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .AddRoles(It.IsAny<int>(), It.IsAny<List<int>>())
                        .Result
                        .Success,
                    Message = UsersMessages.RolesNotAddedMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.RemoveUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .RemoveRoles(It.IsAny<int>(), It.IsAny<List<int>>())
                        .Result
                        .Success,
                    Message = UsersMessages.RolesNotRemovedMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.ActivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .ActivateUser(It.IsAny<int>())
                        .Result,
                    Message = UsersMessages.UserNotActivatedMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.DeactivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .DeactivateUser(It.IsAny<int>())
                        .Result,
                    Message = UsersMessages.UserNotDeactivatedMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(usersService =>
                usersService.ConfirmEmail(It.IsAny<string>()))
                .Returns(Task.FromResult(new ConfirmEmailResult()
                {
                    Success = MockUsersRespository
                        .UsersRepositoryFailedRequest
                        .Object
                        .ConfirmEmail(It.IsAny<string>())
                        .Result
                        .Success,
                    Message = UsersMessages.EmailNotConfirmedMessage
                } as IConfirmEmailResult));
        }
    }
}
