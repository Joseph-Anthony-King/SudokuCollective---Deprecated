using System.Threading.Tasks;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Core.Interfaces.Services
{
    public interface ISolutionsService : IService
    {
        Task<ISolutionResult> Get(int id);
        Task<ISolutionsResult> GetSolutions(IBaseRequest request);
        Task<ISolutionResult> Solve(ISolutionRequest solveRequestsRO);
        Task<ISolutionResult> Generate();
        Task<IBaseResult> Add(int limit);
    }
}
