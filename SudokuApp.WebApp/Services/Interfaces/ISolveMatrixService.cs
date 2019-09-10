using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.RequestObjects.SolveRequests;

namespace SudokuApp.WebApp.Services.Interfaces
{

    public interface ISolveMatrixService {
        
        Task<ActionResult<Game>> Solve(SolveRequestsRO solveRequestsRO);
    }
}