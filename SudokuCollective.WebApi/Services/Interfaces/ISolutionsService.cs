using System.Threading.Tasks;
using SudokuCollective.WebApi.Models.RequestModels;
using SudokuCollective.WebApi.Models.RequestModels.SolveRequests;
using SudokuCollective.WebApi.Models.ResultModels.SolutionRequests;

namespace SudokuCollective.WebApi.Services.Interfaces {

    public interface ISolutionsService {

        Task<SolutionResult> GetSolution(int id, bool fullRecord = false);
        Task<SolutionsResult> GetSolutions(BaseRequest baseRequest, bool fullRecord = false, int userId = 0);
        Task<SolutionResult> Solve(SolveRequest solveRequestsRO);
        Task<SolutionResult> Generate();
    }
}
