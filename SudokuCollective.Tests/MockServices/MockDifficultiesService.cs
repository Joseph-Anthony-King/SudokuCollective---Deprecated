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
using SudokuCollective.Domain.Models;

namespace SudokuCollective.Test.MockServices
{
    public class MockDifficultiesService
    {
        private DatabaseContext _context;
        internal Mock<IDifficultiesService> DifficultiesServiceSuccessfulRequest { get; set; }
        internal Mock<IDifficultiesService> DifficultiesServiceFailedRequest { get; set; }

        public MockDifficultiesService(DatabaseContext context)
        {
            _context = context;
            DifficultiesServiceSuccessfulRequest = new Mock<IDifficultiesService>();
            DifficultiesServiceFailedRequest = new Mock<IDifficultiesService>();

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.GetDifficulty(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultyResult()
                {
                    Success = true,
                    Message = string.Empty,
                    Difficulty = _context.Difficulties.FirstOrDefault(predicate: difficulty => difficulty.Id == 1)
                } as IDifficultyResult));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.GetDifficulties(It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultiesResult()
                {
                    Success = true,
                    Message = string.Empty,
                    Difficulties = _context.Difficulties.ToList()
                } as IDifficultiesResult));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.CreateDifficulty(It.IsAny<string>(), It.IsAny<DifficultyLevel>()))
                .Returns(Task.FromResult(new DifficultyResult()
                {
                    Success = true,
                    Message = string.Empty,
                    Difficulty = new Difficulty(
                        7,
                        "New Difficulty",
                        "New Difficulty",
                        DifficultyLevel.TEST)

                } as IDifficultyResult));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.UpdateDifficulty(It.IsAny<int>(), It.IsAny<UpdateDifficultyRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.DeleteDifficulty(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = true,
                    Message = string.Empty
                } as IBaseResult));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.GetDifficulty(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultyResult()
                {
                    Success = false,
                    Message = "Error retrieving difficulty",
                    Difficulty = new Difficulty()
                } as IDifficultyResult));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.GetDifficulties(It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultiesResult()
                {
                    Success = false,
                    Message = "Error retrieving difficulties",
                    Difficulties = new List<IDifficulty>()
                } as IDifficultiesResult));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.CreateDifficulty(It.IsAny<string>(), It.IsAny<DifficultyLevel>()))
                .Returns(Task.FromResult(new DifficultyResult()
                {
                    Success = false,
                    Message = "Error creating difficulty",
                    Difficulty = new Difficulty()

                } as IDifficultyResult));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.UpdateDifficulty(It.IsAny<int>(), It.IsAny<UpdateDifficultyRequest>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = "Error updating difficulty"
                } as IBaseResult));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.DeleteDifficulty(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult()
                {
                    Success = false,
                    Message = "Error deleting difficulty"
                } as IBaseResult));
        }
    }
}
