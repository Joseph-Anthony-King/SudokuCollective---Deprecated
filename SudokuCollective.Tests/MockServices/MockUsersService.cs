using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.DataModels;
using SudokuCollective.WebApi.Models.PageModels;
using SudokuCollective.WebApi.Models.RequestModels.RegisterRequests;
using SudokuCollective.WebApi.Models.RequestModels.UserRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.MockServices {

    public class MockUsersService {

        private DatabaseContext _context;
        internal Mock<IUsersService> UsersServiceSuccessfulRequest { get; set; }
        internal Mock<IUsersService> UsersServiceFailedRequest { get; set; }

        public MockUsersService(DatabaseContext context) {
            
            _context = context;
            UsersServiceSuccessfulRequest = new Mock<IUsersService>();
            UsersServiceFailedRequest = new Mock<IUsersService>();        

            UsersServiceSuccessfulRequest.Setup(userService => 
                userService.CreateUser(It.IsAny<RegisterRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult() {

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
                }));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.GetUser(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult() {

                    Success = true,
                    Message = string.Empty,
                    User = context.Users.FirstOrDefault(predicate: u => u.Id == 1)
                }));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.GetUsers(It.IsAny<PageListModel>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult() {

                    Success = true,
                    Message = string.Empty,
                    Users = context.Users.ToList()
                }));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.UpdateUser(It.IsAny<int>(), It.IsAny<UpdateUserRequest>()))
                .Returns(Task.FromResult(new UserResult() {

                    Success = true,
                    Message = string.Empty,
                    User = context.Users.FirstOrDefault(predicate: u => u.Id == 1)
                }));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.UpdatePassword(It.IsAny<int>(), It.IsAny<UpdatePasswordRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.DeleteUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.AddUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.RemoveUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.ActivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            UsersServiceSuccessfulRequest.Setup(userService =>
                userService.DeactivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            UsersServiceFailedRequest.Setup(userService => 
                userService.CreateUser(It.IsAny<RegisterRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult() {

                    Success = false,
                    Message = "Error creating user",
                    User = new User()
                }));

            UsersServiceFailedRequest.Setup(userService =>
                userService.GetUser(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult() {

                    Success = false,
                    Message = "Error retrieving user",
                    User = new User()
                }));

            UsersServiceFailedRequest.Setup(userService =>
                userService.GetUsers(It.IsAny<PageListModel>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UsersResult() {

                    Success = false,
                    Message = "Error retrieving users",
                    Users = new List<User>()
                }));

            UsersServiceFailedRequest.Setup(userService =>
                userService.UpdateUser(It.IsAny<int>(), It.IsAny<UpdateUserRequest>()))
                .Returns(Task.FromResult(new UserResult() {

                    Success = false,
                    Message = "Error updating user",
                    User = new User()
                }));

            UsersServiceFailedRequest.Setup(userService =>
                userService.UpdatePassword(It.IsAny<int>(), It.IsAny<UpdatePasswordRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error updating user password"
                }));

            UsersServiceFailedRequest.Setup(userService =>
                userService.DeleteUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error deleting user"
                }));

            UsersServiceFailedRequest.Setup(userService =>
                userService.AddUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error adding role to user"
                }));

            UsersServiceFailedRequest.Setup(userService =>
                userService.RemoveUserRoles(It.IsAny<int>(), It.IsAny<List<int>>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error removing role from user"
                }));

            UsersServiceFailedRequest.Setup(userService =>
                userService.ActivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error activating user"
                }));

            UsersServiceFailedRequest.Setup(userService =>
                userService.DeactivateUser(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error deactivating user"
                }));
        }
    }
}
