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
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Core.Interfaces.Services;

namespace SudokuCollective.Test.MockServices
{
    public class MockDifficultiesService
    {
        private MockDifficultiesRepository MockDifficultiesRepository { get; set; }

        internal Mock<IDifficultiesService> DifficultiesServiceSuccessfulRequest { get; set; }
        internal Mock<IDifficultiesService> DifficultiesServiceFailedRequest { get; set; }

        public MockDifficultiesService(DatabaseContext context)
        {
            MockDifficultiesRepository = new MockDifficultiesRepository(context);

            DifficultiesServiceSuccessfulRequest = new Mock<IDifficultiesService>();
            DifficultiesServiceFailedRequest = new Mock<IDifficultiesService>();

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.GetDifficulty(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultyResult()
                {
                    Success = MockDifficultiesRepository
                        .DifficultiesRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = DifficultiesMessages.DifficultyFoundMessage,
                    Difficulty = (Difficulty)MockDifficultiesRepository
                        .DifficultiesRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Object
                } as IDifficultyResult));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.GetDifficulties(It.IsAny<Paginator>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultiesResult()
                {
                    Success = MockDifficultiesRepository
                        .DifficultiesRepositorySuccessfulRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = DifficultiesMessages.DifficultiesFoundMessage,
                    Difficulties = MockDifficultiesRepository
                        .DifficultiesRepositorySuccessfulRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Objects
                        .ConvertAll(d => (IDifficulty)d)
                } as IDifficultiesResult));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.CreateDifficulty(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DifficultyLevel>()))
                .Returns(Task.FromResult(new DifficultyResult()
                {
                    Success = MockDifficultiesRepository
                        .DifficultiesRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<Difficulty>())
                        .Result
                        .Success,
                    Message = DifficultiesMessages.DifficultyCreatedMessage,
                    Difficulty = (Difficulty)MockDifficultiesRepository
                        .DifficultiesRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<Difficulty>())
                        .Result
                        .Object
                } as IDifficultyResult));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.UpdateDifficulty(It.IsAny<int>(), It.IsAny<UpdateDifficultyRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockDifficultiesRepository
                        .DifficultiesRepositorySuccessfulRequest
                        .Object
                        .Update(It.IsAny<Difficulty>())
                        .Result
                        .Success,
                    Message = DifficultiesMessages.DifficultyUpdatedMessage
                } as IBaseResult));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.DeleteDifficulty(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockDifficultiesRepository
                        .DifficultiesRepositorySuccessfulRequest
                        .Object
                        .Delete(It.IsAny<Difficulty>())
                        .Result
                        .Success,
                    Message = DifficultiesMessages.DifficultyDeletedMessage
                } as IBaseResult));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.GetDifficulty(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultyResult()
                {
                    Success = MockDifficultiesRepository
                        .DifficultiesRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<Difficulty>())
                        .Result
                        .Success,
                    Message = DifficultiesMessages.DifficultyNotFoundMessage,
                    Difficulty = (Difficulty)MockDifficultiesRepository
                        .DifficultiesRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<Difficulty>())
                        .Result
                        .Object
                } as IDifficultyResult));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.GetDifficulties(It.IsAny<Paginator>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultiesResult()
                {
                    Success = MockDifficultiesRepository
                        .DifficultiesRepositoryFailedRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = DifficultiesMessages.DifficultiesNotFoundMessage,
                    Difficulties = null
                } as IDifficultiesResult));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.CreateDifficulty(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<DifficultyLevel>()))
                .Returns(Task.FromResult(new DifficultyResult()
                {
                    Success = MockDifficultiesRepository
                        .DifficultiesRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<Difficulty>())
                        .Result
                        .Success,
                    Message = DifficultiesMessages.DifficultyNotCreatedMessage,
                    Difficulty = (Difficulty)MockDifficultiesRepository
                        .DifficultiesRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<Difficulty>())
                        .Result
                        .Object
                } as IDifficultyResult));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.UpdateDifficulty(It.IsAny<int>(), It.IsAny<UpdateDifficultyRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockDifficultiesRepository
                        .DifficultiesRepositoryFailedRequest
                        .Object
                        .Update(It.IsAny<Difficulty>())
                        .Result
                        .Success,
                    Message = DifficultiesMessages.DifficultyNotUpdatedMessage
                } as IBaseResult));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.DeleteDifficulty(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = MockDifficultiesRepository
                        .DifficultiesRepositoryFailedRequest
                        .Object
                        .Delete(It.IsAny<Difficulty>())
                        .Result
                        .Success,
                    Message = DifficultiesMessages.DifficultyNotDeletedMessage
                } as IBaseResult));
        }
    }
}
