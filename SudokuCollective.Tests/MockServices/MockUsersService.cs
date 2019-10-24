using System;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels.RegisterRequests;
using SudokuCollective.WebApi.Models.ResultModels.UserRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.MockServices {

    public class MockUsersService {

        private DatabaseContext _context;
        internal Mock<IUsersService> UsersServiceSuccessfulRequest { get; set; }
        internal Mock<IUsersService> UsersServiceFailedRequest { get; set; }
        internal Mock<IUsersService> UsersServiceInvalidRequest { get; set; }

        public MockUsersService(DatabaseContext context) {
            
            _context = context;
            UsersServiceSuccessfulRequest = new Mock<IUsersService>();
            UsersServiceFailedRequest = new Mock<IUsersService>();
            UsersServiceInvalidRequest = new Mock<IUsersService>();            

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

            UsersServiceFailedRequest.Setup(userService => 
                userService.CreateUser(It.IsAny<RegisterRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new UserResult() {

                    Success = false,
                    Message = "Error creating user",
                    User = new User()
                }));
        }
    }
}