using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.SolveRequests;
using SudokuCollective.WebApi.Models.ResultModels.SolutionResults;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.Tests.MockServices {

    public class MockSolutionsService {

        private DatabaseContext _context;
        internal Mock<ISolutionsService> SolutionsServiceSuccessfulRequest { get; set; }
        internal Mock<ISolutionsService> SolutionsServiceFailedRequest { get; set; }

        public MockSolutionsService(DatabaseContext context) {

            _context = context;
            SolutionsServiceSuccessfulRequest = new Mock<ISolutionsService>();
            SolutionsServiceFailedRequest = new Mock<ISolutionsService>();

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.GetSolution(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new SolutionResult() {

                    Success = true,
                    Message = string.Empty,
                    Solution = _context.SudokuSolutions.FirstOrDefault(predicate: solution => solution.Id == 1)
                }));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.GetSolutions(It.IsAny<BaseRequest>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new SolutionsResult() {

                    Success = true,
                    Message = string.Empty,
                    Solutions = _context.SudokuSolutions.ToList()
                }));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.Solve(It.IsAny<SolveRequest>()))
                .Returns(Task.FromResult(new SolutionResult() {

                    Success = true,
                    Message = string.Empty,
                    Solution = _context.SudokuSolutions.FirstOrDefault(predicate: solution => solution.Id == 1)
                }));

            SolutionsServiceSuccessfulRequest.Setup(solutionsService =>
                solutionsService.Generate()).Returns(
                    Task.FromResult(new SolutionResult(){

                            Success = true,
                            Message = string.Empty,
                            Solution = _context.SudokuSolutions.FirstOrDefault(predicate: solution => solution.Id == 1)
                        }));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.GetSolution(It.IsAny<int>(), It.IsAny<bool>()))
                .Returns(Task.FromResult(new SolutionResult() {

                    Success = false,
                    Message = "Error retrieving solution",
                    Solution = new SudokuSolution()
                }));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.GetSolutions(It.IsAny<BaseRequest>(), It.IsAny<bool>(), It.IsAny<int>()))
                .Returns(Task.FromResult(new SolutionsResult() {

                    Success = false,
                    Message = "Error retrieving solutions",
                    Solutions = new List<SudokuSolution>()
                }));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.Solve(It.IsAny<SolveRequest>()))
                .Returns(Task.FromResult(new SolutionResult() {

                    Success = false,
                    Message = "Error solving sudoku matrix",
                    Solution = new SudokuSolution()
                }));

            SolutionsServiceFailedRequest.Setup(solutionsService =>
                solutionsService.Generate()).Returns(
                    Task.FromResult(new SolutionResult() {

                        Success = false,
                        Message = "Error generating solution",
                        Solution = new SudokuSolution()
                    }));
        }
    }
}
