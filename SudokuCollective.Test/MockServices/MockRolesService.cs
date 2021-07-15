using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.MockRepositories;
using SudokuCollective.Data.Messages;
using SudokuCollective.Core.Interfaces.Services;

namespace SudokuCollective.Test.MockServices
{
    public class MockRolesService
    {
        private MockRolesRepository MockRolesRepository { get; set; }

        internal Mock<IRolesService> RolesServiceSuccessfulRequest { get; set; }
        internal Mock<IRolesService> RolesServiceFailedRequest { get; set; }

        public MockRolesService(DatabaseContext context)
        {
            MockRolesRepository = new MockRolesRepository(context);

            RolesServiceSuccessfulRequest = new Mock<IRolesService>();
            RolesServiceFailedRequest = new Mock<IRolesService>();

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.Get(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new RoleResult()
                {
                    Success = MockRolesRepository
                        .RolesRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = RolesMessages.RoleFoundMessage,
                    Role = (Role)MockRolesRepository
                        .RolesRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Object
                } as IRoleResult));

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.GetRoles(It.IsAny<bool>()))
                .Returns(Task.FromResult(new RolesResult()
                {
                    Success = MockRolesRepository
                        .RolesRepositorySuccessfulRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = RolesMessages.RolesFoundMessage,
                    Roles = MockRolesRepository
                        .RolesRepositorySuccessfulRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Objects
                        .ConvertAll(r => (IRole)r)
                } as IRolesResult));

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.Create(It.IsAny<string>(), It.IsAny<RoleLevel>()))
                .Returns(Task.FromResult(new RoleResult()
                {
                    Success = MockRolesRepository
                        .RolesRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<Role>())
                        .Result
                        .Success,
                    Message = RolesMessages.RoleCreatedMessage,
                    Role = (Role)MockRolesRepository
                        .RolesRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<Role>())
                        .Result
                        .Object
                } as IRoleResult));

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.Update(It.IsAny<int>(), It.IsAny<UpdateRoleRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockRolesRepository
                        .RolesRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<Role>())
                        .Result
                        .Success,
                    Message = RolesMessages.RoleUpdatedMessage
                } as IBaseResult));

            RolesServiceSuccessfulRequest.Setup(rolesService =>
                rolesService.Delete(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockRolesRepository
                        .RolesRepositorySuccessfulRequest
                        .Object
                        .Delete(It.IsAny<Role>())
                        .Result
                        .Success,
                    Message = RolesMessages.RoleDeletedMessage
                } as IBaseResult));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.Get(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new RoleResult()
                {
                    Success = MockRolesRepository
                        .RolesRepositoryFailedRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = RolesMessages.RoleNotFoundMessage,
                    Role = (Role)MockRolesRepository
                        .RolesRepositoryFailedRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Object
                } as IRoleResult));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.GetRoles(It.IsAny<bool>()))
                .Returns(Task.FromResult(new RolesResult()
                {
                    Success = MockRolesRepository
                        .RolesRepositoryFailedRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = RolesMessages.RolesNotFoundMessage,
                    Roles = null
                } as IRolesResult));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.Create(It.IsAny<string>(), It.IsAny<RoleLevel>()))
                .Returns(Task.FromResult(new RoleResult()
                {
                    Success = MockRolesRepository
                        .RolesRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<Role>())
                        .Result
                        .Success,
                    Message = RolesMessages.RoleNotCreatedMessage,
                    Role = (Role)MockRolesRepository
                        .RolesRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<Role>())
                        .Result
                        .Object
                } as IRoleResult));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.Update(It.IsAny<int>(), It.IsAny<UpdateRoleRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockRolesRepository
                        .RolesRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<Role>())
                        .Result
                        .Success,
                    Message = RolesMessages.RoleNotUpdatedMessage
                } as IBaseResult));

            RolesServiceFailedRequest.Setup(rolesService =>
                rolesService.Delete(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockRolesRepository
                        .RolesRepositoryFailedRequest
                        .Object
                        .Delete(It.IsAny<Role>())
                        .Result
                        .Success,
                    Message = RolesMessages.RoleNotDeletedMessage
                } as IBaseResult));
        }
    }
}
