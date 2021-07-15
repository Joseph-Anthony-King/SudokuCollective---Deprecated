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
using System.Threading.Tasks;

namespace SudokuCollective.Test.MockRepositories
{
    public class MockSolutionsRepository
    {
        private readonly DatabaseContext context;
        internal Mock<ISolutionsRepository<SudokuSolution>> SolutionsRepositorySuccessfulRequest { get; set; }
        internal Mock<ISolutionsRepository<SudokuSolution>> SolutionsRepositoryFailedRequest { get; set; }

        public MockSolutionsRepository(DatabaseContext ctxt)
        {
            context = ctxt;

            SolutionsRepositorySuccessfulRequest = new Mock<ISolutionsRepository<SudokuSolution>>();
            SolutionsRepositoryFailedRequest = new Mock<ISolutionsRepository<SudokuSolution>>();

            SolutionsRepositorySuccessfulRequest.Setup(solutionsRepo =>
                solutionsRepo.Create(It.IsAny<SudokuSolution>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Object = new SudokuSolution()
                    } as IRepositoryResponse));

            SolutionsRepositorySuccessfulRequest.Setup(solutionsRepo =>
                solutionsRepo.Get(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Object = context.SudokuSolutions.FirstOrDefault(s => s.Id == 1)
                    } as IRepositoryResponse));

            SolutionsRepositorySuccessfulRequest.Setup(solutionsRepo =>
                solutionsRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.SudokuSolutions.ToList().ConvertAll(s => (IEntityBase)s)
                    } as IRepositoryResponse));

            SolutionsRepositorySuccessfulRequest.Setup(solutionsRepo =>
                solutionsRepo.AddSolutions(It.IsAny<List<ISudokuSolution>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            SolutionsRepositorySuccessfulRequest.Setup(solutionsRepo =>
                solutionsRepo.GetSolvedSolutions())
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.SudokuSolutions
                            .Where(s => s.DateSolved > DateTime.MinValue)
                            .ToList()
                            .ConvertAll(s => (IEntityBase)s)
                    } as IRepositoryResponse));

            SolutionsRepositoryFailedRequest.Setup(solutionsRepo =>
                solutionsRepo.Create(It.IsAny<SudokuSolution>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            SolutionsRepositoryFailedRequest.Setup(solutionsRepo =>
                solutionsRepo.Get(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            SolutionsRepositoryFailedRequest.Setup(solutionsRepo =>
                solutionsRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            SolutionsRepositoryFailedRequest.Setup(solutionsRepo =>
                solutionsRepo.AddSolutions(It.IsAny<List<ISudokuSolution>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            SolutionsRepositoryFailedRequest.Setup(solutionsRepo =>
                solutionsRepo.GetSolvedSolutions())
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));
        }
    }
}
