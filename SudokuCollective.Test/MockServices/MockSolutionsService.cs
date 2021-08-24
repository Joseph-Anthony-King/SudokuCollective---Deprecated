using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Test.MockRepositories;
using SudokuCollective.Data.Messages;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;

namespace SudokuCollective.Test.MockServices
{
    public class MockSolutionsService
    {
        private MockSolutionsRepository MockSolutionsRepository { get; set; }
        private MockUsersRepository MockUsersRepository { get; set; }

        internal Mock<ISolutionsService> SolutionsServiceSuccessfulRequest { get; set; }
        internal Mock<ISolutionsService> SolutionsServiceFailedRequest { get; set; }
        internal Mock<ISolutionsService> SolutionsServiceSolveFailedRequest { get; set; }

        public MockSolutionsService(DatabaseContext context)
        {
            MockSolutionsRepository = new MockSolutionsRepository(context);
            MockUsersRepository = new MockUsersRepository(context);

            SolutionsServiceSuccessfulRequest = new Mock<ISolutionsService>();
            SolutionsServiceFailedRequest = new Mock<ISolutionsService>();
            SolutionsServiceSolveFailedRequest = new Mock<ISolutionsService>();

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.Get(It.IsAny<int>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    IsSuccess = MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = SolutionsMessages.SolutionFoundMessage,
                    Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Object
                } as ISolutionResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.GetSolutions(It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new SolutionsResult()
                {
                    IsSuccess = MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = SolutionsMessages.SolutionsFoundMessage,
                    Solutions = MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .GetAll()
                        .Result
                        .Objects
                        .ConvertAll(s => (ISudokuSolution)s)
                } as ISolutionsResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.Solve(It.IsAny<ISolutionRequest>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    IsSuccess = true,
                    Message = SolutionsMessages.SudokuSolutionFoundMessage,
                    Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<SudokuSolution>())
                        .Result
                        .Object
                } as ISolutionResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.Generate()).Returns(
                    Task.FromResult(new SolutionResult()
                    {
                        IsSuccess = MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<SudokuSolution>())
                        .Result
                        .Success,
                        Message = SolutionsMessages.SolutionGeneratedMessage,
                        Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .Add(It.IsAny<SudokuSolution>())
                        .Result
                        .Object
                    } as ISolutionResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.Add(It.IsAny<int>())).Returns(
                    Task.FromResult(new BaseResult()
                    {
                        IsSuccess = MockSolutionsRepository
                            .SolutionsRepositorySuccessfulRequest
                            .Object
                            .AddSolutions(It.IsAny<List<ISudokuSolution>>())
                            .Result
                            .Success,
                        Message = SolutionsMessages.SolutionsAddedMessage
                    } as IBaseResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.Get(It.IsAny<int>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    IsSuccess = MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Success,
                    Message = SolutionsMessages.SolutionNotFoundMessage,
                    Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .Get(It.IsAny<int>())
                        .Result
                        .Object
                } as ISolutionResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.GetSolutions(It.IsAny<BaseRequest>()))
                .Returns(Task.FromResult(new SolutionsResult()
                {
                    IsSuccess = MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .GetAll()
                        .Result
                        .Success,
                    Message = SolutionsMessages.SolutionsNotFoundMessage,
                    Solutions = null
                } as ISolutionsResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.Solve(It.IsAny<ISolutionRequest>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    IsSuccess = false,
                    Message = SolutionsMessages.SudokuSolutionNotFoundMessage,
                    Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<SudokuSolution>())
                        .Result
                        .Object
                } as ISolutionResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.Generate()).Returns(
                    Task.FromResult(new SolutionResult()
                    {
                        IsSuccess = MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<SudokuSolution>())
                        .Result
                        .Success,
                        Message = SolutionsMessages.SolutionNotGeneratedMessage,
                        Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .Add(It.IsAny<SudokuSolution>())
                        .Result
                        .Object
                    } as ISolutionResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.Add(It.IsAny<int>())).Returns(
                    Task.FromResult(new BaseResult()
                    {
                        IsSuccess = MockSolutionsRepository
                            .SolutionsRepositoryFailedRequest
                            .Object
                            .AddSolutions(It.IsAny<List<ISudokuSolution>>())
                            .Result
                            .Success,
                        Message = SolutionsMessages.SolutionsNotAddedMessage
                    } as IBaseResult));

            SolutionsServiceSolveFailedRequest.Setup(solutionsService =>
                solutionsService.Solve(It.IsAny<ISolutionRequest>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    IsSuccess = true,
                    Message = SolutionsMessages.SudokuSolutionNotFoundMessage,
                    Solution = null
                } as ISolutionResult));
        }
    }
}
