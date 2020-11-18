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
                solutionsService.GetSolution(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    Success = MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = SolutionsMessages.SolutionFoundMessage,
                    Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Object
                } as ISolutionResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.GetSolutions(It.IsAny<BaseRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new SolutionsResult()
                {
                    Success = MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = SolutionsMessages.SolutionsFoundMessage,
                    Solutions = MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Objects
                        .ConvertAll(s => (ISudokuSolution)s)
                } as ISolutionsResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.Solve(It.IsAny<SolveRequest>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    Success = true,
                    Message = SolutionsMessages.SolutionSolvedMessage,
                    Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .Create(It.IsAny<SudokuSolution>())
                        .Result
                        .Object
                } as ISolutionResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.Generate()).Returns(
                    Task.FromResult(new SolutionResult()
                    {
                        Success = MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .Create(It.IsAny<SudokuSolution>())
                        .Result
                        .Success,
                        Message = SolutionsMessages.SolutionGeneratedMessage,
                        Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositorySuccessfulRequest
                        .Object
                        .Create(It.IsAny<SudokuSolution>())
                        .Result
                        .Object
                    } as ISolutionResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.AddSolutions(It.IsAny<int>())).Returns(
                    Task.FromResult(new BaseResult()
                    {
                        Success = MockSolutionsRepository
                            .SolutionsRepositorySuccessfulRequest
                            .Object
                            .AddSolutions(It.IsAny<List<ISudokuSolution>>())
                            .Result
                            .Success,
                        Message = SolutionsMessages.SolutionsAddedMessage
                    } as IBaseResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.GetSolution(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    Success = MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = SolutionsMessages.SolutionNotFoundMessage,
                    Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .GetById(It.IsAny<int>(), It.IsAny<bool>())
                        .Result
                        .Object
                } as ISolutionResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.GetSolutions(It.IsAny<BaseRequest>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new SolutionsResult()
                {
                    Success = MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .GetAll(It.IsAny<bool>())
                        .Result
                        .Success,
                    Message = SolutionsMessages.SolutionsNotFoundMessage,
                    Solutions = null
                } as ISolutionsResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.Solve(It.IsAny<SolveRequest>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    Success = false,
                    Message = SolutionsMessages.SolutionNotSolvedMessage,
                    Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .Create(It.IsAny<SudokuSolution>())
                        .Result
                        .Object
                } as ISolutionResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.Generate()).Returns(
                    Task.FromResult(new SolutionResult()
                    {
                        Success = MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .Create(It.IsAny<SudokuSolution>())
                        .Result
                        .Success,
                        Message = SolutionsMessages.SolutionNotGeneratedMessage,
                        Solution = (SudokuSolution)MockSolutionsRepository
                        .SolutionsRepositoryFailedRequest
                        .Object
                        .Create(It.IsAny<SudokuSolution>())
                        .Result
                        .Object
                    } as ISolutionResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.AddSolutions(It.IsAny<int>())).Returns(
                    Task.FromResult(new BaseResult()
                    {
                        Success = MockSolutionsRepository
                            .SolutionsRepositoryFailedRequest
                            .Object
                            .AddSolutions(It.IsAny<List<ISudokuSolution>>())
                            .Result
                            .Success,
                        Message = SolutionsMessages.SolutionsNotAddedMessage
                    } as IBaseResult));

            SolutionsServiceSolveFailedRequest.Setup(solutionsService =>
                solutionsService.Solve(It.IsAny<SolveRequest>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    Success = true,
                    Message = SolutionsMessages.SolutionNotSolvedMessage,
                    Solution = null
                } as ISolutionResult));
        }
    }
}
