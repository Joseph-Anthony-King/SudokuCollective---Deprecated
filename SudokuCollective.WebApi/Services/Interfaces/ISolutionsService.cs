using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Models;
using SudokuCollective.WebApi.Models.RequestModels.SolveRequests;

namespace SudokuCollective.WebApi.Services.Interfaces
{

    public interface ISolutionsService {

        Task<ActionResult<SudokuSolution>> GetSolution(int id, bool fullRecord = true);
        Task<ActionResult<IEnumerable<SudokuSolution>>> GetSolutions(bool fullRecord = true);
        Task<ActionResult<SudokuSolution>> Solve(SolveRequestsRO solveRequestsRO);
    }
}