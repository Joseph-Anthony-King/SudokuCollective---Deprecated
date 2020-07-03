using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.WebApi.Models.TokenModels;
using SudokuCollective.WebApi.Models.DTOModels;
using SudokuCollective.WebApi.Models.ResultModels.UserResults;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase {

        private readonly IAuthenticateService _authService;

        public AuthenticateController(IAuthenticateService authService) {

            _authService = authService;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public ActionResult RequestToken([FromBody] TokenRequest request) {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            }

            if (_authService.IsAuthenticated(request, out string token, out AuthenticatedUser user)) {

                var result = new AuthenticatedUserResult(user, token) { 
                    Success = true 
                };

                return Ok(result);
            }

            return BadRequest("Status Code 400: Invalid User");
        }
    }
}
