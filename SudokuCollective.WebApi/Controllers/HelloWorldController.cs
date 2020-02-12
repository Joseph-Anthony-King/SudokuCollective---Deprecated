using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize]
    [Route("api/v1/helloworld")]
    [ApiController]
    public class HelloWorldController : ControllerBase {
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Get() {

            return Ok("Hello World from Sudoku Collective!");
        }
    }
}
