using System.Threading.Tasks;
using SudokuCollective.WebApi.Models.RequestModels.SolveRequests;
using SudokuCollective.WebApi.Models.TaskModels.SolutionRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface ISolutionsService {

        Task<SolutionTaskResult> GetSolution(int id, bool fullRecord = true);
        Task<SolutionListTaskResult> GetSolutions(bool fullRecord = true);
        Task<SolutionTaskResult> Solve(SolveRequestsRO solveRequestsRO);
        Task<SolutionTaskResult> Generate();
    }
}
