using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Api.Controllers
{
    [Authorize]
    [Route("api/v1/helloworld")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Get()
        {
            var result = new BaseResult()
            {
                Success = true,
                Message = "Status Code 200: Hello World from Sudoku Collective!"
            };
            return Ok(result);
        }
    }
}
