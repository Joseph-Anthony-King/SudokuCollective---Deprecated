using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels.RoleRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.RoleRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.MockServices {

    public class MockRolesService {

        private DatabaseContext _context;
        internal Mock<IRolesService> RolesServiceSuccessfulRequest { get; set; }
        internal Mock<IRolesService> RolesServiceFailedRequest { get; set; }

        public MockRolesService(DatabaseContext context) {

            _context = context;
            RolesServiceSuccessfulRequest = new Mock<IRolesService>();
            RolesServiceFailedRequest = new Mock<IRolesService>();

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.GetRole(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new RoleResult() {

                    Success = true,
                    Message = string.Empty,
                    Role = _context.Roles.FirstOrDefault(predicate: role => role.Id == 1)
                }));

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.GetRoles(It.IsAny<bool>()))
                .Returns(Task.FromResult(new RolesResult() {

                    Success = true,
                    Message = string.Empty,
                    Roles = _context.Roles.ToList()
                }));

            RolesServiceSuccessfulRequest.Setup(rolesService => 
                rolesService.CreateRole(It.IsAny<string>(), It.IsAny<RoleLevel>()))
                .Returns(Task.FromResult(new RoleResult() {

                    Success = true,
                    Message = string.Empty,
                    Role = new Role(5, "New Role", RoleLevel.NULL)
                }));

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.UpdateRole(It.IsAny<int>(), It.IsAny<UpdateRoleRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.DeleteRole(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.GetRole(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new RoleResult() {

                    Success = false,
                    Message = "Error retrieving role",
                    Role = new Role()
                }));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.GetRoles(It.IsAny<bool>()))
                .Returns(Task.FromResult(new RolesResult() {

                    Success = false,
                    Message = "Error retrieving roles",
                    Roles = new List<Role>()
                }));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.CreateRole(It.IsAny<string>(), It.IsAny<RoleLevel>()))
                .Returns(Task.FromResult(new RoleResult() {

                    Success = false,
                    Message = "Error creating role",
                    Role = new Role()
                }));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.UpdateRole(It.IsAny<int>(), It.IsAny<UpdateRoleRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error updating role"
                }));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.DeleteRole(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error deleting role"
                }));
        }
    }
}
