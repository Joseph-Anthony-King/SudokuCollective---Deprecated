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
    public class MockPasswordUpdatesRepository
    {
        private readonly DatabaseContext context;
        internal Mock<IPasswordUpdatesRepository<PasswordUpdate>> PasswordUpdatesRepositorySuccessfulRequest { get; set; }
        internal Mock<IPasswordUpdatesRepository<PasswordUpdate>> PasswordUpdatesRepositoryFailedRequest { get; set; }

        public MockPasswordUpdatesRepository(DatabaseContext ctxt)
        {
            context = ctxt;

            PasswordUpdatesRepositorySuccessfulRequest = new Mock<IPasswordUpdatesRepository<PasswordUpdate>>();
            PasswordUpdatesRepositoryFailedRequest = new Mock<IPasswordUpdatesRepository<PasswordUpdate>>();

            PasswordUpdatesRepositorySuccessfulRequest.Setup(passwordUpdatesRepo =>
                passwordUpdatesRepo.Create(It.IsAny<PasswordUpdate>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = new EmailConfirmation()
                    } as IRepositoryResponse));

            PasswordUpdatesRepositorySuccessfulRequest.Setup(passwordUpdatesRepo =>
                passwordUpdatesRepo.Get(It.IsAny<string>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.EmailConfirmations.FirstOrDefault(ec => ec.Id == 1)
                    } as IRepositoryResponse));

            PasswordUpdatesRepositorySuccessfulRequest.Setup(passwordUpdatesRepo =>
                passwordUpdatesRepo.GetAll())
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.EmailConfirmations
                            .ToList()
                            .ConvertAll(d => (IEntityBase)d)
                    } as IRepositoryResponse));

            PasswordUpdatesRepositorySuccessfulRequest.Setup(passwordUpdatesRepo =>
                passwordUpdatesRepo.Delete(It.IsAny<PasswordUpdate>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            PasswordUpdatesRepositorySuccessfulRequest.Setup(passwordUpdatesRepo =>
                passwordUpdatesRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));


            PasswordUpdatesRepositoryFailedRequest.Setup(passwordUpdatesRepo =>
                passwordUpdatesRepo.Create(It.IsAny<PasswordUpdate>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            PasswordUpdatesRepositoryFailedRequest.Setup(passwordUpdatesRepo =>
                passwordUpdatesRepo.Get(It.IsAny<string>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            PasswordUpdatesRepositoryFailedRequest.Setup(passwordUpdatesRepo =>
                passwordUpdatesRepo.GetAll())
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            PasswordUpdatesRepositoryFailedRequest.Setup(passwordUpdatesRepo =>
                passwordUpdatesRepo.Delete(It.IsAny<PasswordUpdate>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            PasswordUpdatesRepositoryFailedRequest.Setup(passwordUpdatesRepo =>
                passwordUpdatesRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));
        }
    }
}
