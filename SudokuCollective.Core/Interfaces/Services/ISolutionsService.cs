using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface ISolutionsService : IService
    {
        Task<ISolutionResult> GetSolution(int id, bool fullRecord = true);
        Task<ISolutionsResult> GetSolutions(IBaseRequest request, bool fullRecord = true);
        Task<ISolutionResult> Solve(ISolutionRequest solveRequestsRO);
        Task<ISolutionResult> Generate();
        Task<IBaseResult> AddSolutions(int limit);
    }
}
