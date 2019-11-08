using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.DifficultyRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.MockServices {

    public class MockDifficultiesService {

        private DatabaseContext _context;
        internal Mock<IDifficultiesService> DifficultiesServiceSuccessfulRequest { get; set; }
        internal Mock<IDifficultiesService> DifficultiesServiceFailedRequest { get; set; }

        public MockDifficultiesService(DatabaseContext context) {

            _context = context;
            DifficultiesServiceSuccessfulRequest = new Mock<IDifficultiesService>();
            DifficultiesServiceFailedRequest = new Mock<IDifficultiesService>();

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService => 
                difficultiesService.GetDifficulty(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultyResult() {

                    Success = true,
                    Message = string.Empty,
                    Difficulty = _context.Difficulties.FirstOrDefault(predicate: difficulty => difficulty.Id == 1)
                }));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.GetDifficulties(It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultiesResult() {

                    Success = true,
                    Message = string.Empty,
                    Difficulties = _context.Difficulties.ToList()
                }));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService =>
                difficultiesService.CreateDifficulty(It.IsAny<string>(), It.IsAny<DifficultyLevel>()))
                .Returns(Task.FromResult(new DifficultyResult() {

                    Success = true,
                    Message = string.Empty,
                    Difficulty = new Difficulty(
                        7, 
                        "New Difficulty", 
                        "New Difficulty", 
                        DifficultyLevel.TEST)

                }));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService => 
                difficultiesService.UpdateDifficulty(It.IsAny<int>(), It.IsAny<UpdateDifficultyRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            DifficultiesServiceSuccessfulRequest.Setup(difficultiesService => 
                difficultiesService.DeleteDifficulty(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = true,
                    Message = string.Empty
                }));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.GetDifficulty(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultyResult() {

                    Success = false,
                    Message = "Error retrieving difficulty",
                    Difficulty = new Difficulty()
                }));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.GetDifficulties(It.IsAny<bool>()))
                .Returns(Task.FromResult(new DifficultiesResult() {

                    Success = false,
                    Message = "Error retrieving difficulties",
                    Difficulties = new List<Difficulty>()
                }));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.CreateDifficulty(It.IsAny<string>(), It.IsAny<DifficultyLevel>()))
                .Returns(Task.FromResult(new DifficultyResult() {

                    Success = false,
                    Message = "Error creating difficulty",
                    Difficulty = new Difficulty()

                }));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.UpdateDifficulty(It.IsAny<int>(), It.IsAny<UpdateDifficultyRequest>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error updating difficulty"
                }));

            DifficultiesServiceFailedRequest.Setup(difficultiesService =>
                difficultiesService.DeleteDifficulty(It.IsAny<int>()))
                .Returns(Task.FromResult(new BaseResult() {

                    Success = false,
                    Message = "Error deleting difficulty"
                }));
        }
    }
}
