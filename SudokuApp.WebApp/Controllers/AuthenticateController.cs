using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.WebApp.Models;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase {

        private readonly IAuthenticateService _authService;

        public AuthenticateController(IAuthenticateService authService) {

            _authService = authService;
        }
        
        [AllowAnonymous]
        [HttpPost, Route("request")]
        public ActionResult RequestToken([FromBody] TokenRequest request) {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            }

            if (_authService.IsAuthenticated(request, out string token)) {

                return Ok(token);
            }

            return BadRequest("Status Code 400: Invalid User");
        }
    }
}