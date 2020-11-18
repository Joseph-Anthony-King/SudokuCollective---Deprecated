using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Test.MockRepositories
{
    public class MockRolesRepository
    {
        private readonly DatabaseContext context;
        internal Mock<IRolesRepository<Role>> RolesRepositorySuccessfulRequest { get; set; }
        internal Mock<IRolesRepository<Role>> RolesRepositoryFailedRequest { get; set; }

        public MockRolesRepository(DatabaseContext ctxt)
        {
            context = ctxt;

            RolesRepositorySuccessfulRequest = new Mock<IRolesRepository<Role>>();
            RolesRepositoryFailedRequest = new Mock<IRolesRepository<Role>>();

            RolesRepositorySuccessfulRequest.Setup(rolesRepo =>
                rolesRepo.Create(It.IsAny<Role>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Object = new Role() { RoleLevel = RoleLevel.NULL }
                    } as IRepositoryResponse));

            RolesRepositorySuccessfulRequest.Setup(rolesRepo =>
                rolesRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Object = context.Roles.FirstOrDefault(predicate: r => r.Id == 3)
                    } as IRepositoryResponse));

            RolesRepositorySuccessfulRequest.Setup(rolesRepo =>
                rolesRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Objects = context.Roles
                            .Where(r => r.RoleLevel != RoleLevel.NULL && r.RoleLevel != RoleLevel.SUPERUSER)
                            .ToList()
                            .ConvertAll(r => (IEntityBase)r)
                    } as IRepositoryResponse));

            RolesRepositorySuccessfulRequest.Setup(rolesRepo =>
                rolesRepo.Update(It.IsAny<Role>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Roles.FirstOrDefault(predicate: r => r.Id == 3)
                    } as IRepositoryResponse));

            RolesRepositorySuccessfulRequest.Setup(rolesRepo =>
                rolesRepo.UpdateRange(It.IsAny<List<Role>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            RolesRepositorySuccessfulRequest.Setup(rolesRepo =>
                rolesRepo.Delete(It.IsAny<Role>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            RolesRepositorySuccessfulRequest.Setup(rolesRepo =>
                rolesRepo.DeleteRange(It.IsAny<List<Role>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            RolesRepositorySuccessfulRequest.Setup(rolesRepo =>
                rolesRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            RolesRepositorySuccessfulRequest.Setup(rolesRepo =>
                rolesRepo.HasRoleLevel(It.IsAny<RoleLevel>()))
                    .Returns(Task.FromResult(true));

            RolesRepositorySuccessfulRequest.Setup(rolesRepo =>
                rolesRepo.IsListValid(It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(true));

            RolesRepositoryFailedRequest.Setup(rolesRepo =>
                rolesRepo.Create(It.IsAny<Role>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            RolesRepositoryFailedRequest.Setup(rolesRepo =>
                rolesRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            RolesRepositoryFailedRequest.Setup(rolesRepo =>
                rolesRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            RolesRepositoryFailedRequest.Setup(rolesRepo =>
                rolesRepo.Update(It.IsAny<Role>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            RolesRepositoryFailedRequest.Setup(rolesRepo =>
                rolesRepo.UpdateRange(It.IsAny<List<Role>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            RolesRepositoryFailedRequest.Setup(rolesRepo =>
                rolesRepo.Delete(It.IsAny<Role>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            RolesRepositoryFailedRequest.Setup(rolesRepo =>
                rolesRepo.DeleteRange(It.IsAny<List<Role>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            RolesRepositoryFailedRequest.Setup(rolesRepo =>
                rolesRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            RolesRepositoryFailedRequest.Setup(rolesRepo =>
                rolesRepo.HasRoleLevel(It.IsAny<RoleLevel>()))
                    .Returns(Task.FromResult(false));

            RolesRepositoryFailedRequest.Setup(rolesRepo =>
                rolesRepo.IsListValid(It.IsAny<List<int>>()))
                    .Returns(Task.FromResult(false));
        }
    }
}
