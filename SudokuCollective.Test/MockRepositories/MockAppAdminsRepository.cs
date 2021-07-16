using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;

namespace SudokuCollective.Test.MockRepositories
{
    public class MockAppAdminsRepository
    {
        private readonly DatabaseContext context;
        internal Mock<IAppAdminsRepository<AppAdmin>> AppAdminsRepositorySuccessfulRequest { get; set; }
        internal Mock<IAppAdminsRepository<AppAdmin>> AppAdminsRepositoryFailedRequest { get; set; }
        internal Mock<IAppAdminsRepository<AppAdmin>> AppAdminsRepositoryPromoteUser { get; set; }

        public MockAppAdminsRepository(DatabaseContext ctxt)
        {
            context = ctxt;

            AppAdminsRepositorySuccessfulRequest = new Mock<IAppAdminsRepository<AppAdmin>>();
            AppAdminsRepositoryFailedRequest = new Mock<IAppAdminsRepository<AppAdmin>>();
            AppAdminsRepositoryPromoteUser = new Mock<IAppAdminsRepository<AppAdmin>>();

            AppAdminsRepositorySuccessfulRequest.Setup(appAdminsRepo =>
                appAdminsRepo.Add(It.IsAny<AppAdmin>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Object = new AppAdmin() { 
                            Id = 2,
                            AppId = 1,
                            UserId = 2,
                            IsActive = true }
                    } as IRepositoryResponse));

            AppAdminsRepositorySuccessfulRequest.Setup(appAdminsRepo =>
                appAdminsRepo.Get(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.AppAdmins.FirstOrDefault(aa => aa.Id == 1)
                    } as IRepositoryResponse));

            AppAdminsRepositorySuccessfulRequest.Setup(appAdminsRepo =>
                appAdminsRepo.GetAll())
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Objects = context.AppAdmins
                            .ToList()
                            .ConvertAll(aa => (IEntityBase)aa)
                    } as IRepositoryResponse));

            AppAdminsRepositorySuccessfulRequest.Setup(appAdminsRepo =>
                appAdminsRepo.Update(It.IsAny<AppAdmin>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Difficulties.FirstOrDefault(d => d.Id == 1)
                    } as IRepositoryResponse));

            AppAdminsRepositorySuccessfulRequest.Setup(appAdminsRepo =>
                appAdminsRepo.UpdateRange(It.IsAny<List<AppAdmin>>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    { 
                        Success = true
                    } as IRepositoryResponse));

            AppAdminsRepositorySuccessfulRequest.Setup(appAdminsRepo =>
                appAdminsRepo.Delete(It.IsAny<AppAdmin>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppAdminsRepositorySuccessfulRequest.Setup(appAdminsRepo =>
                appAdminsRepo.DeleteRange(It.IsAny<List<AppAdmin>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppAdminsRepositorySuccessfulRequest.Setup(appAdminsRepo =>
                appAdminsRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            AppAdminsRepositorySuccessfulRequest.Setup(appAdminsRepo =>
                appAdminsRepo.HasAdminRecord(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            AppAdminsRepositorySuccessfulRequest.Setup(appAdminsRepo =>
                appAdminsRepo.GetAdminRecord(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.AppAdmins.FirstOrDefault(aa => aa.Id == 1)
                    } as IRepositoryResponse));

            AppAdminsRepositoryFailedRequest.Setup(appAdminsRepo =>
                appAdminsRepo.Add(It.IsAny<AppAdmin>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppAdminsRepositoryFailedRequest.Setup(appAdminsRepo =>
                appAdminsRepo.Get(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppAdminsRepositoryFailedRequest.Setup(appAdminsRepo =>
                appAdminsRepo.GetAll())
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppAdminsRepositoryFailedRequest.Setup(appAdminsRepo =>
                appAdminsRepo.Update(It.IsAny<AppAdmin>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppAdminsRepositoryFailedRequest.Setup(appAdminsRepo =>
                appAdminsRepo.UpdateRange(It.IsAny<List<AppAdmin>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppAdminsRepositoryFailedRequest.Setup(appAdminsRepo =>
                appAdminsRepo.Delete(It.IsAny<AppAdmin>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppAdminsRepositoryFailedRequest.Setup(appAdminsRepo =>
                appAdminsRepo.DeleteRange(It.IsAny<List<AppAdmin>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppAdminsRepositoryFailedRequest.Setup(appAdminsRepo =>
                appAdminsRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            AppAdminsRepositoryFailedRequest.Setup(appAdminsRepo =>
                appAdminsRepo.HasAdminRecord(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            AppAdminsRepositoryFailedRequest.Setup(appAdminsRepo =>
                appAdminsRepo.GetAdminRecord(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            AppAdminsRepositoryPromoteUser.Setup(appAdminsRepo =>
                appAdminsRepo.Add(It.IsAny<AppAdmin>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = new AppAdmin()
                        {
                            Id = 2,
                            AppId = 1,
                            UserId = 2,
                            IsActive = true
                        }
                    } as IRepositoryResponse));

            AppAdminsRepositoryPromoteUser.Setup(appAdminsRepo =>
                appAdminsRepo.Get(It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.AppAdmins.FirstOrDefault(aa => aa.Id == 1)
                    } as IRepositoryResponse));

            AppAdminsRepositoryPromoteUser.Setup(appAdminsRepo =>
                appAdminsRepo.GetAll())
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.AppAdmins
                            .ToList()
                            .ConvertAll(aa => (IEntityBase)aa)
                    } as IRepositoryResponse));

            AppAdminsRepositoryPromoteUser.Setup(appAdminsRepo =>
                appAdminsRepo.Update(It.IsAny<AppAdmin>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Difficulties.FirstOrDefault(d => d.Id == 1)
                    } as IRepositoryResponse));

            AppAdminsRepositoryPromoteUser.Setup(appAdminsRepo =>
                appAdminsRepo.UpdateRange(It.IsAny<List<AppAdmin>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppAdminsRepositoryPromoteUser.Setup(appAdminsRepo =>
                appAdminsRepo.Delete(It.IsAny<AppAdmin>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppAdminsRepositoryPromoteUser.Setup(appAdminsRepo =>
                appAdminsRepo.DeleteRange(It.IsAny<List<AppAdmin>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            AppAdminsRepositoryPromoteUser.Setup(appAdminsRepo =>
                appAdminsRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            AppAdminsRepositoryPromoteUser.Setup(appAdminsRepo =>
                appAdminsRepo.HasAdminRecord(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            AppAdminsRepositoryPromoteUser.Setup(appAdminsRepo =>
                appAdminsRepo.GetAdminRecord(It.IsAny<int>(), It.IsAny<int>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.AppAdmins.FirstOrDefault(aa => aa.Id == 1)
                    } as IRepositoryResponse));
        }
    }
}
