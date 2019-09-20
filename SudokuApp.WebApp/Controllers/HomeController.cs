using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace SudokuApp.WebApp.Controllers {

    [Authorize]
    [Route("")]
    public class HomeController : Controller {

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Get() {

            return View("index");
        }        
    }
}
