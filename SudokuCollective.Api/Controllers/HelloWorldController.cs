using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.ResultModels;

namespace SudokuCollective.Api.Controllers
{
    [Authorize]
    [Route("api/helloworld")]
    [ApiController]
    public class HelloWorldController : ControllerBase
    {
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Get()
        {
            var result = new BaseResult
            {
                IsSuccess = true,
                Message = ControllerMessages.StatusCode200("Hello World from Sudoku Collective!")
            };

            return Ok(result);
        }
    }
}
