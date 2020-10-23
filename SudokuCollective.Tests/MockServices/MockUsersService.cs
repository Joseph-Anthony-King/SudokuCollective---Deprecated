using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.Test.MockServices
{
    public class MockUsersService
    {
        private DatabaseContext _context;
        internal Mock<IUsersService> UsersServiceSuccessfulRequest { get; set; }
        internal Mock<IUsersService> UsersServiceFailedRequest { get; set; }

        public MockUsersService(DatabaseContext context)
        {
            _context = context;
            UsersServiceSuccessfulRequest = new Mock<IUsersService>();
            UsersServiceFailedRequest = new Mock<IUsersService>();

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.CreateUser(It.IsAny<RegisterRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = true,
                    Message = string.Empty,
                    User = new User(
                        3,
                        "Test User 3",
                        "Test",
                        "User 3",
                        "My Nickname",
                        "testuser3@example.com",
                        "password1",
                        true,
                        DateTime.UtcNow,
                        DateTime.UtcNow)
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.GetUser(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = true,
                    Message = string.Empty,
                    User = context.Users.FirstOrDefault(predicate: u => u.Id == 1)
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.GetUsers(It.IsAny<PageListModel>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {
                    Success = true,
                    Message = string.Empty,
                    Users = context.Users.ToList()
                } as IUsersResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.UpdateUser(It.IsAny<int>(), It.IsAny<UpdateUserRequest>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = true,
                    Message = string.Empty,
                    User = context.Users.FirstOrDefault(predicate: u => u.Id == 1)
                } as IUserResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.UpdatePassword(It.IsAny<int>(), It.IsAny<UpdatePasswordRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.DeleteUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.AddUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.RemoveUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.ActivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.DeactivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.CreateUser(It.IsAny<RegisterRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = false,
                    Message = "Error creating user",
                    User = new User()
                } as IUserResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.GetUser(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = false,
                    Message = "Error retrieving user",
                    User = new User()
                } as IUserResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.GetUsers(It.IsAny<PageListModel>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult()
                {

                    Success = false,
                    Message = "Error retrieving users",
                    Users = new List<IUser>()
                } as IUsersResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.UpdateUser(It.IsAny<int>(), It.IsAny<UpdateUserRequest>()))
                .Returns(Task.FromResult(new UserResult()
                {
                    Success = false,
                    Message = "Error updating user",
                    User = new User()
                } as IUserResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.UpdatePassword(It.IsAny<int>(), It.IsAny<UpdatePasswordRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = "Error updating user password"
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.DeleteUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = "Error deleting user"
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.AddUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = "Error adding role to user"
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.RemoveUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = "Error removing role from user"
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.ActivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = "Error activating user"
                } as IBaseResult));

            UsersServiceFailedRequest.Setup(userService =>
                userService.DeactivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = "Error deactivating user"
                } as IBaseResult));
        }
    }
}
