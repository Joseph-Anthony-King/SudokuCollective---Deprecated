using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Test.MockServices
{
    public class MockSolutionsService
    {
        private DatabaseContext _context;
        internal Mock<ISolutionsService> SolutionsServiceSuccessfulRequest { get; set; }
        internal Mock<ISolutionsService> SolutionsServiceFailedRequest { get; set; }
        internal Mock<ISolutionsService> SolutionsServiceSolveFailedRequest { get; set; }

        public MockSolutionsService(DatabaseContext context)
        {
            _context = context;
            SolutionsServiceSuccessfulRequest = new Mock<ISolutionsService>();
            SolutionsServiceFailedRequest = new Mock<ISolutionsService>();
            SolutionsServiceSolveFailedRequest = new Mock<ISolutionsService>();

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.GetSolution(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    Success = true,
                    Message = string.Empty,
                    Solution = _context.SudokuSolutions.FirstOrDefault(predicate: solution => solution.Id == 1)
                } as ISolutionResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.GetSolutions(It.IsAny<BaseRequest>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new SolutionsResult()
                {
                    Success = true,
                    Message = string.Empty,
                    Solutions = (_context.SudokuSolutions.ToList()).ConvertAll(s => s as ISudokuSolution)
                } as ISolutionsResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.Solve(It.IsAny<SolveRequest>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    Success = true,
                    Message = string.Empty,
                    Solution = _context.SudokuSolutions.FirstOrDefault(predicate: solution => solution.Id == 1)
                } as ISolutionResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.Generate()).Returns(
                    Task.FromResult(new SolutionResult()
                    {
                        Success = true,
                        Message = string.Empty,
                        Solution = _context.SudokuSolutions.FirstOrDefault(predicate: solution => solution.Id == 1)
                    } as ISolutionResult));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.AddSolutions(It.IsAny<int>())).Returns(
                    Task.FromResult(new BaseResult()
                    {
                        Success = true,
                        Message = string.Empty
                    } as IBaseResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.GetSolution(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    Success = false,
                    Message = "Error retrieving solution",
                    Solution = new SudokuSolution()
                } as ISolutionResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.GetSolutions(It.IsAny<BaseRequest>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new SolutionsResult()
                {
                    Success = false,
                    Message = "Error retrieving solutions",
                    Solutions = new List<ISudokuSolution>()
                } as ISolutionsResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.Solve(It.IsAny<SolveRequest>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    Success = false,
                    Message = "Error solving sudoku matrix",
                    Solution = new SudokuSolution()
                } as ISolutionResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.Generate()).Returns(
                    Task.FromResult(new SolutionResult()
                    {
                        Success = false,
                        Message = "Error generating solution",
                        Solution = new SudokuSolution()
                    } as ISolutionResult));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.AddSolutions(It.IsAny<int>())).Returns(
                    Task.FromResult(new BaseResult()
                    {
                        Success = false,
                        Message = "Error generating solutions"
                    } as IBaseResult));

            SolutionsServiceSolveFailedRequest.Setup(solutionsService =>
                solutionsService.Solve(It.IsAny<SolveRequest>()))
                .Returns(Task.FromResult(new SolutionResult()
                {
                    Success = true,
                    Message = string.Empty,
                    Solution = null
                } as ISolutionResult));
        }
    }
}
