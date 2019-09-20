using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SudokuApp.WebApi.Controllers {

    [Authorize]
    [Route("api/helloworld")]
    [ApiController]
    public class HelloWorldController : ControllerBase {
        
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Get() {

            return Ok("Hello World from Sudoku Collective!");
        }
    }
}