using Moq;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Repositories;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.DataModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SudokuCollective.Test.MockRepositories
{
    public class MockEmailConfirmationsRepository
    {
        private readonly DatabaseContext context;
        internal Mock<IEmailConfirmationsRepository<EmailConfirmation>> EmailConfirmationsRepositorySuccessfulRequest { get; set; }
        internal Mock<IEmailConfirmationsRepository<EmailConfirmation>> EmailConfirmationsRepositoryFailedRequest { get; set; }

        public MockEmailConfirmationsRepository(DatabaseContext ctxt)
        {
            context = ctxt;

            EmailConfirmationsRepositorySuccessfulRequest = new Mock<IEmailConfirmationsRepository<EmailConfirmation>>();
            EmailConfirmationsRepositoryFailedRequest = new Mock<IEmailConfirmationsRepository<EmailConfirmation>>();

            EmailConfirmationsRepositorySuccessfulRequest.Setup(emailConfirmationsRepo =>
                emailConfirmationsRepo.Create(It.IsAny<EmailConfirmation>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = new EmailConfirmation()
                    } as IRepositoryResponse));

            EmailConfirmationsRepositorySuccessfulRequest.Setup(emailConfirmationsRepo =>
                emailConfirmationsRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.EmailConfirmations.FirstOrDefault(ec => ec.Id == 1)
                    } as IRepositoryResponse));

            EmailConfirmationsRepositorySuccessfulRequest.Setup(emailConfirmationsRepo =>
                emailConfirmationsRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.EmailConfirmations
                            .ToList()
                            .ConvertAll(d => (IEntityBase)d)
                    } as IRepositoryResponse));

            EmailConfirmationsRepositorySuccessfulRequest.Setup(emailConfirmationsRepo =>
                emailConfirmationsRepo.Update(It.IsAny<EmailConfirmation>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Difficulties.FirstOrDefault(ec => ec.Id == 1)
                    } as IRepositoryResponse));

            EmailConfirmationsRepositorySuccessfulRequest.Setup(emailConfirmationsRepo =>
                emailConfirmationsRepo.UpdateRange(It.IsAny<List<EmailConfirmation>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            EmailConfirmationsRepositorySuccessfulRequest.Setup(emailConfirmationsRepo =>
                emailConfirmationsRepo.Delete(It.IsAny<EmailConfirmation>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            EmailConfirmationsRepositorySuccessfulRequest.Setup(emailConfirmationsRepo =>
                emailConfirmationsRepo.DeleteRange(It.IsAny<List<EmailConfirmation>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            EmailConfirmationsRepositorySuccessfulRequest.Setup(emailConfirmationsRepo =>
                emailConfirmationsRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            EmailConfirmationsRepositorySuccessfulRequest.Setup(emailConfirmationsRepo =>
                emailConfirmationsRepo.GetByToken(It.IsAny<string>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.EmailConfirmations.FirstOrDefault(ec => ec.Id == 1)
                    } as IRepositoryResponse));


            EmailConfirmationsRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Create(It.IsAny<EmailConfirmation>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            EmailConfirmationsRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            EmailConfirmationsRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            EmailConfirmationsRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Update(It.IsAny<EmailConfirmation>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            EmailConfirmationsRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.UpdateRange(It.IsAny<List<EmailConfirmation>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            EmailConfirmationsRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Delete(It.IsAny<EmailConfirmation>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            EmailConfirmationsRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.DeleteRange(It.IsAny<List<EmailConfirmation>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            EmailConfirmationsRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            EmailConfirmationsRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.GetByToken(It.IsAny<string>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));
        }
    }
}
