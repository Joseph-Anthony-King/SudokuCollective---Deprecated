using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.WebApi.Models;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize]
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