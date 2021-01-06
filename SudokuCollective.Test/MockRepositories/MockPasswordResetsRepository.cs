﻿using System.Linq;
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
    public class MockPasswordResetsRepository
    {
        private readonly DatabaseContext context;
        internal Mock<IPasswordResetsRepository<PasswordReset>> PasswordResetsRepositorySuccessfulRequest { get; set; }
        internal Mock<IPasswordResetsRepository<PasswordReset>> PasswordResetsRepositoryFailedRequest { get; set; }

        public MockPasswordResetsRepository(DatabaseContext ctxt)
        {
            context = ctxt;

            PasswordResetsRepositorySuccessfulRequest = new Mock<IPasswordResetsRepository<PasswordReset>>();
            PasswordResetsRepositoryFailedRequest = new Mock<IPasswordResetsRepository<PasswordReset>>();

            PasswordResetsRepositorySuccessfulRequest.Setup(passwordResetsRepo =>
                passwordResetsRepo.Create(It.IsAny<PasswordReset>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = new EmailConfirmation()
                    } as IRepositoryResponse));

            PasswordResetsRepositorySuccessfulRequest.Setup(passwordResetsRepo =>
                passwordResetsRepo.Get(It.IsAny<string>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.EmailConfirmations.FirstOrDefault(ec => ec.Id == 1)
                    } as IRepositoryResponse));

            PasswordResetsRepositorySuccessfulRequest.Setup(passwordResetsRepo =>
                passwordResetsRepo.GetAll())
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.EmailConfirmations
                            .ToList()
                            .ConvertAll(d => (IEntityBase)d)
                    } as IRepositoryResponse));

            PasswordResetsRepositorySuccessfulRequest.Setup(passwordResetsRepo =>
                passwordResetsRepo.Delete(It.IsAny<PasswordReset>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            PasswordResetsRepositorySuccessfulRequest.Setup(passwordResetsRepo =>
                passwordResetsRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));


            PasswordResetsRepositoryFailedRequest.Setup(passwordResetsRepo =>
                passwordResetsRepo.Create(It.IsAny<PasswordReset>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            PasswordResetsRepositoryFailedRequest.Setup(passwordResetsRepo =>
                passwordResetsRepo.Get(It.IsAny<string>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            PasswordResetsRepositoryFailedRequest.Setup(passwordResetsRepo =>
                passwordResetsRepo.GetAll())
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            PasswordResetsRepositoryFailedRequest.Setup(passwordResetsRepo =>
                passwordResetsRepo.Delete(It.IsAny<PasswordReset>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            PasswordResetsRepositoryFailedRequest.Setup(passwordResetsRepo =>
                passwordResetsRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));
        }
    }
}