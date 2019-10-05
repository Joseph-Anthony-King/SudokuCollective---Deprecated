using System.Threading.Tasks;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.SolveRequests;
using SudokuCollective.WebApi.Models.TaskModels.SolutionRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface ISolutionsService {

        Task<SolutionTaskResult> GetSolution(int id, bool fullRecord = false);
        Task<SolutionListTaskResult> GetSolutions(BaseRequestRO baseRequest, bool fullRecord = false, int userId = 0);
        Task<SolutionTaskResult> Solve(SolveRequestsRO solveRequestsRO);
        Task<SolutionTaskResult> Generate();
    }
}
