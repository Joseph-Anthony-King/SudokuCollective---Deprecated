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
    public class MockDifficultiesRepository
    {
        private readonly DatabaseContext context;
        internal Mock<IDifficultiesRepository<Difficulty>> DifficultiesRepositorySuccessfulRequest { get; set; }
        internal Mock<IDifficultiesRepository<Difficulty>> DifficultiesRepositoryFailedRequest { get; set; }
        internal Mock<IDifficultiesRepository<Difficulty>> DifficultiesRepositoryCreateDifficultyRequest { get; set; }

        public MockDifficultiesRepository(DatabaseContext ctxt)
        {
            context = ctxt;

            DifficultiesRepositorySuccessfulRequest = new Mock<IDifficultiesRepository<Difficulty>>();
            DifficultiesRepositoryFailedRequest = new Mock<IDifficultiesRepository<Difficulty>>();
            DifficultiesRepositoryCreateDifficultyRequest = new Mock<IDifficultiesRepository<Difficulty>>();

            DifficultiesRepositorySuccessfulRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Create(It.IsAny<Difficulty>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Object = new Difficulty() { DifficultyLevel = DifficultyLevel.NULL }
                    } as IRepositoryResponse));

            DifficultiesRepositorySuccessfulRequest.Setup(difficultiesRepo =>
                difficultiesRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Difficulties.FirstOrDefault(d => d.Id == 4)
                    } as IRepositoryResponse));

            DifficultiesRepositorySuccessfulRequest.Setup(difficultiesRepo =>
                difficultiesRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    {
                        Success = true,
                        Objects = context.Difficulties
                            .Where(d => 
                                d.DifficultyLevel != DifficultyLevel.NULL && d.DifficultyLevel != DifficultyLevel.TEST)
                            .ToList()
                            .ConvertAll(d => (IEntityBase)d)
                    } as IRepositoryResponse));

            DifficultiesRepositorySuccessfulRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Update(It.IsAny<Difficulty>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Difficulties.FirstOrDefault(d => d.Id == 4)
                    } as IRepositoryResponse));

            DifficultiesRepositorySuccessfulRequest.Setup(difficultiesRepo =>
                difficultiesRepo.UpdateRange(It.IsAny<List<Difficulty>>()))
                    .Returns(Task.FromResult(new RepositoryResponse() 
                    { 
                        Success = true
                    } as IRepositoryResponse));

            DifficultiesRepositorySuccessfulRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Delete(It.IsAny<Difficulty>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            DifficultiesRepositorySuccessfulRequest.Setup(difficultiesRepo =>
                difficultiesRepo.DeleteRange(It.IsAny<List<Difficulty>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            DifficultiesRepositorySuccessfulRequest.Setup(difficultiesRepo =>
                difficultiesRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            DifficultiesRepositorySuccessfulRequest.Setup(difficultiesRepo =>
                difficultiesRepo.HasDifficultyLevel(It.IsAny<DifficultyLevel>()))
                    .Returns(Task.FromResult(true));

            DifficultiesRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Create(It.IsAny<Difficulty>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            DifficultiesRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            DifficultiesRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            DifficultiesRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Update(It.IsAny<Difficulty>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            DifficultiesRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.UpdateRange(It.IsAny<List<Difficulty>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            DifficultiesRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Delete(It.IsAny<Difficulty>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            DifficultiesRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.DeleteRange(It.IsAny<List<Difficulty>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = false
                    } as IRepositoryResponse));

            DifficultiesRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(false));

            DifficultiesRepositoryFailedRequest.Setup(difficultiesRepo =>
                difficultiesRepo.HasDifficultyLevel(It.IsAny<DifficultyLevel>()))
                    .Returns(Task.FromResult(false));

            DifficultiesRepositoryCreateDifficultyRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Create(It.IsAny<Difficulty>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = new Difficulty() { DifficultyLevel = DifficultyLevel.NULL }
                    } as IRepositoryResponse));

            DifficultiesRepositoryCreateDifficultyRequest.Setup(difficultiesRepo =>
                difficultiesRepo.GetById(It.IsAny<int>(), It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Difficulties.FirstOrDefault(d => d.Id == 4)
                    } as IRepositoryResponse));

            DifficultiesRepositoryCreateDifficultyRequest.Setup(difficultiesRepo =>
                difficultiesRepo.GetAll(It.IsAny<bool>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Objects = context.Difficulties
                            .Where(d =>
                                d.DifficultyLevel != DifficultyLevel.NULL && d.DifficultyLevel != DifficultyLevel.TEST)
                            .ToList()
                            .ConvertAll(d => (IEntityBase)d)
                    } as IRepositoryResponse));

            DifficultiesRepositoryCreateDifficultyRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Update(It.IsAny<Difficulty>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true,
                        Object = context.Difficulties.FirstOrDefault(d => d.Id == 4)
                    } as IRepositoryResponse));

            DifficultiesRepositoryCreateDifficultyRequest.Setup(difficultiesRepo =>
                difficultiesRepo.UpdateRange(It.IsAny<List<Difficulty>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            DifficultiesRepositoryCreateDifficultyRequest.Setup(difficultiesRepo =>
                difficultiesRepo.Delete(It.IsAny<Difficulty>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            DifficultiesRepositoryCreateDifficultyRequest.Setup(difficultiesRepo =>
                difficultiesRepo.DeleteRange(It.IsAny<List<Difficulty>>()))
                    .Returns(Task.FromResult(new RepositoryResponse()
                    {
                        Success = true
                    } as IRepositoryResponse));

            DifficultiesRepositoryCreateDifficultyRequest.Setup(difficultiesRepo =>
                difficultiesRepo.HasEntity(It.IsAny<int>()))
                    .Returns(Task.FromResult(true));

            DifficultiesRepositoryCreateDifficultyRequest.Setup(difficultiesRepo =>
                difficultiesRepo.HasDifficultyLevel(It.IsAny<DifficultyLevel>()))
                    .Returns(Task.FromResult(false));
        }
    }
}
