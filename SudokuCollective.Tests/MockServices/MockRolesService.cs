using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Test.MockServices
{
    public class MockRolesService
    {
        private DatabaseContext _context;
        internal Mock<IRolesService> RolesServiceSuccessfulRequest { get; set; }
        internal Mock<IRolesService> RolesServiceFailedRequest { get; set; }

        public MockRolesService(DatabaseContext context)
        {
            _context = context;
            RolesServiceSuccessfulRequest = new Mock<IRolesService>();
            RolesServiceFailedRequest = new Mock<IRolesService>();

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.GetRole(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new RoleResult()
                {
                    Success = true,
                    Message = string.Empty,
                    Role = _context.Roles.FirstOrDefault(predicate: role => role.Id == 1)
                } as IRoleResult));

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.GetRoles(It.IsAny<bool>()))
                .Returns(Task.FromResult(new RolesResult()
                {
                    Success = true,
                    Message = string.Empty,
                    Roles = (_context.Roles.ToList()).ConvertAll(r => r as IRole)
                } as IRolesResult));

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.CreateRole(It.IsAny<string>(), It.IsAny<RoleLevel>()))
                .Returns(Task.FromResult(new RoleResult()
                {
                    Success = true,
                    Message = string.Empty,
                    Role = new Role(5, "New Role", RoleLevel.NULL)
                } as IRoleResult));

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.UpdateRole(It.IsAny<int>(), It.IsAny<UpdateRoleRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.DeleteRole(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.GetRole(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new RoleResult()
                {
                    Success = false,
                    Message = "Error retrieving role",
                    Role = new Role()
                } as IRoleResult));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.GetRoles(It.IsAny<bool>()))
                .Returns(Task.FromResult(new RolesResult()
                {
                    Success = false,
                    Message = "Error retrieving roles",
                    Roles = new List<IRole>()
                } as IRolesResult));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.CreateRole(It.IsAny<string>(), It.IsAny<RoleLevel>()))
                .Returns(Task.FromResult(new RoleResult()
                {
                    Success = false,
                    Message = "Error creating role",
                    Role = new Role()
                } as IRoleResult));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.UpdateRole(It.IsAny<int>(), It.IsAny<UpdateRoleRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = "Error updating role"
                } as IBaseResult));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.DeleteRole(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = "Error deleting role"
                } as IBaseResult));
        }
    }
}
