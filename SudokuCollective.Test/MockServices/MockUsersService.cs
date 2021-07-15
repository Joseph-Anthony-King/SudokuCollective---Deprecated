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
using System;

namespace SudokuCollective.Test.MockServices
{
    public class MockUsersService
    {
        private MockUsersRepository MockUsersRepository { get; set; }
        private MockAppsRepository MockAppsRepository { get; set; }
        private MockPasswordResetsRepository MockPasswordResetsRepository { get; set; }
        private MockEmailConfirmationsRepository MockEmailConfirmationsRepository { get; set; }

        internal Mock<IUsersService> UsersServiceSuccessfulRequest { get; set; }
        internal Mock<IUsersService> UsersServiceFailedRequest { get; set; }

        public MockUsersService(DatabaseContext context)
        {
            MockUsersRepository = new MockUsersRepository(context);
            MockAppsRepository = new MockAppsRepository(context);
            MockPasswordResetsRepository = new MockPasswordResetsRepository(context);
            MockEmailConfirmationsRepository = new MockEmailConfirmationsRepository(context);

            UsersServiceSuccessfulRequest = new Mock<IUsersService>();
            UsersServiceFailedRequest = new Mock<IUsersService>();

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.Create(It.IsAny<RegisterRequest>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserCreatedMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.Get(
                    It.IsAny<int>(),
                    It.IsAny<string>(), 
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserFoundMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.GetUsers(
                    It.IsAny<int>(),
                    It.IsAny<string>(), 
                    It.IsAny<Paginator>(), 
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = UsersMessages.UsersFoundMessage,
                    Users = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Objects
                        .ConvertAll(u => (IUser)u)
                } as IUsersResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.Update(
                    It.IsAny<int>(), 
                    It.IsAny<UpdateUserRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserUpdatedMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.RequestPasswordReset(
                    It.IsAny<RequestPasswordResetRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = UsersMessages.ProcessedPasswordResetRequestMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.ResendPasswordReset(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = UsersMessages.PasswordResetEmailResentMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.UpdatePassword(It.IsAny<UpdatePasswordRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.PasswordResetMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.Delete(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Delete(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserDeletedMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.AddUserRoles(
                    It.IsAny<int>(), 
                    It.IsAny<List<int>>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new RolesResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .AddRoles(It.IsAny<int>(), It.IsAny<List<int>>())
                        .Result
                        .Success,
                    Message = UsersMessages.RolesAddedMessage,
                    Roles = new List<IRole>()
                } as IRolesResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.RemoveUserRoles(
                    It.IsAny<int>(), 
                    It.IsAny<List<int>>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .RemoveRoles(It.IsAny<int>(), It.IsAny<List<int>>())
                        .Result
                        .Success,
                    Message = UsersMessages.RolesRemovedMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.Activate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Activate(It.IsAny<int>())
                        .Result,
                    Message = UsersMessages.UserActivatedMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.Deactivate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .Deactivate(It.IsAny<int>())
                        .Result,
                    Message = UsersMessages.UserDeactivatedMessage
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(usersService =>
                usersService.ConfirmEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new ConfirmEmailResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .ConfirmEmail(It.IsAny<IEmailConfirmation>())
                        .Result
                        .Success,
                    Message = UsersMessages.EmailConfirmedMessage
                } as IConfirmEmailResult));

            UsersServiceSuccessfulRequest.Setup(usersService =>
                usersService.InitiatePasswordReset(It.IsAny<string>()))
                .Returns(Task.FromResult(new InitiatePasswordResetResult() 
                {
                    Success = MockPasswordResetsRepository
                        .PasswordResetsRepositorySuccessfulRequest
                        .Object
                        .Create(It.IsAny<PasswordReset>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserFoundMessage,
                    Token = Guid.NewGuid().ToString(),
                    ConfirmationEmailSuccessfullySent = true,
                    App = (App)MockAppsRepository
                        .AppsRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>())
                        .Result
                        .Object,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>())
                        .Result
                        .Object
                } as IInitiatePasswordResetResult));

            UsersServiceSuccessfulRequest.Setup(usersService =>
                usersService.ResendEmailConfirmation(
                    It.IsAny<int>(), 
                    It.IsAny<int>(), 
                    It.IsAny<string>(), 
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new UserResult() 
                { 
                    Success = MockEmailConfirmationsRepository
                        .EmailConfirmationsRepositorySuccessfulRequest
                        .Object
                        .HasOutstandingEmailConfirmation(It.IsAny<int>(), It.IsAny<int>())
                        .Result,
                    Message = UsersMessages.EmailConfirmationEmailResentMessage,
                    ConfirmationEmailSuccessfullySent = true
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(usersService =>
                usersService.CancelEmailConfirmationRequest(
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult() 
                {
                    Success = MockEmailConfirmationsRepository
                        .EmailConfirmationsRepositorySuccessfulRequest
                        .Object
                        .Delete(It.IsAny<EmailConfirmation>())
                        .Result
                        .Success,
                    Message = UsersMessages.EmailConfirmationRequestCancelledMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(usersService =>
                usersService.CancelPasswordResetRequest(
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockPasswordResetsRepository
                        .PasswordResetsRepositorySuccessfulRequest
                        .Object
                        .Delete(It.IsAny<PasswordReset>())
                        .Result
                        .Success,
                    Message = UsersMessages.PasswordResetRequestCancelledMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(usersService =>
                usersService.CancelAllEmailRequests(
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = true,
                    Message = string.Format("{0} and {1}", 
                        UsersMessages.EmailConfirmationRequestCancelledMessage, 
                        UsersMessages.PasswordResetRequestCancelledMessage),
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.Create(It.IsAny<RegisterRequest>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserNotCreatedMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.Get(
                    It.IsAny<int>(), 
                    It.IsAny<string>(),
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserNotFoundMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Object,
                } as IUserResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.GetUsers(
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<Paginator>(), 
                    It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = UsersMessages.UsersNotFoundMessage,
                    Users = null
                } as IUsersResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.Update(
                    It.IsAny<int>(), 
                    It.IsAny<UpdateUserRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserNotUpdatedMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.RequestPasswordReset(
                    It.IsAny<RequestPasswordResetRequest>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = UsersMessages.UnableToProcessPasswordResetRequesMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.ResendPasswordReset(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = UsersMessages.UserNotFoundMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.UpdatePassword(It.IsAny<UpdatePasswordRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.PasswordNotResetMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.Delete(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Delete(It.IsAny<User>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserNotDeletedMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.AddUserRoles(
                    It.IsAny<int>(), 
                    It.IsAny<List<int>>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new RolesResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .AddRoles(It.IsAny<int>(), It.IsAny<List<int>>())
                        .Result
                        .Success,
                    Message = UsersMessages.RolesNotAddedMessage,
                    Roles = new List<IRole>()
                } as IRolesResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.RemoveUserRoles(
                    It.IsAny<int>(), 
                    It.IsAny<List<int>>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .RemoveRoles(It.IsAny<int>(), It.IsAny<List<int>>())
                        .Result
                        .Success,
                    Message = UsersMessages.RolesNotRemovedMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.Activate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Activate(It.IsAny<int>())
                        .Result,
                    Message = UsersMessages.UserNotActivatedMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.Deactivate(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .Deactivate(It.IsAny<int>())
                        .Result,
                    Message = UsersMessages.UserNotDeactivatedMessage
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(usersService =>
                usersService.ConfirmEmail(
                    It.IsAny<string>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new ConfirmEmailResult()
                {
                    Success = MockUsersRepository
                        .UsersRepositoryFailedRequest
                        .Object
                        .ConfirmEmail(It.IsAny<IEmailConfirmation>())
                        .Result
                        .Success,
                    Message = UsersMessages.EmailNotConfirmedMessage
                } as IConfirmEmailResult));

            UsersServiceFailedRequest.Setup(usersService =>
                usersService.InitiatePasswordReset(It.IsAny<string>()))
                .Returns(Task.FromResult(new InitiatePasswordResetResult()
                {
                    Success = MockPasswordResetsRepository
                        .PasswordResetsRepositoryFailedRequest
                        .Object
                        .Create(It.IsAny<PasswordReset>())
                        .Result
                        .Success,
                    Message = UsersMessages.UserNotFoundMessage,
                    Token = null,
                    ConfirmationEmailSuccessfullySent = null,
                    App = new App(),
                    User = new User()
                } as IInitiatePasswordResetResult));

            UsersServiceFailedRequest.Setup(usersService =>
                usersService.ResendEmailConfirmation(
                    It.IsAny<int>(),
                    It.IsAny<int>(),
                    It.IsAny<string>(),
                    It.IsAny<string>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockEmailConfirmationsRepository
                        .EmailConfirmationsRepositoryFailedRequest
                        .Object
                        .HasOutstandingEmailConfirmation(It.IsAny<int>(), It.IsAny<int>())
                        .Result,
                    Message = UsersMessages.EmailConfirmationEmailNotResentMessage,
                    ConfirmationEmailSuccessfullySent = false
                } as IUserResult));

            UsersServiceFailedRequest.Setup(usersService =>
                usersService.CancelEmailConfirmationRequest(
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockEmailConfirmationsRepository
                        .EmailConfirmationsRepositoryFailedRequest
                        .Object
                        .Delete(It.IsAny<EmailConfirmation>())
                        .Result
                        .Success,
                    Message = UsersMessages.EmailConfirmationRequestNotCancelledMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceFailedRequest.Setup(usersService =>
                usersService.CancelPasswordResetRequest(
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = MockPasswordResetsRepository
                        .PasswordResetsRepositoryFailedRequest
                        .Object
                        .Delete(It.IsAny<PasswordReset>())
                        .Result
                        .Success,
                    Message = UsersMessages.PasswordResetRequestNotCancelledMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>())
                        .Result
                        .Object
                } as IUserResult));

            UsersServiceFailedRequest.Setup(usersService =>
                usersService.CancelAllEmailRequests(
                    It.IsAny<int>(),
                    It.IsAny<int>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = false,
                    Message = UsersMessages.EmailRequestsNotFoundMessage,
                    User = (User)MockUsersRepository
                        .UsersRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>())
                        .Result
                        .Object
                } as IUserResult));
        }
    }
}
