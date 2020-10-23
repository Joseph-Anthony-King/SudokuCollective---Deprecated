using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.TokenModels;
using SudokuCollective.Core.Interfaces.APIModels.DTOModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Enums;

namespace SudokuCollective.Api.Controllers
{

    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase {

        private readonly IAuthenticateService _authService;
        private readonly IUserManagementService _userManagementService;

        public AuthenticateController(
            IAuthenticateService authService, 
            IUserManagementService userManagementService) {

            _authService = authService;
            _userManagementService = userManagementService;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RequestToken([FromBody] TokenRequest request) {

            if (!ModelState.IsValid) {

                return BadRequest(ModelState);
            }

            if (_authService.IsAuthenticated(
                request, 
                out string token, 
                out IAuthenticatedUser user)) {

                var result = new AuthenticatedUserResult()
                {
                    Success = true,
                    Message = string.Empty,
                    User = user,
                    Token = token
                };

                return Ok(result);

            } else {

                var result = await _userManagementService
                    .ConfirmAuthenticationIssue(request.UserName, request.Password);

                if (result == UserAuthenticationErrorType.USERNAMEINVALID)
                {
                    return BadRequest("Status Code 400: User Name Invalid");
                }
                else if (result == UserAuthenticationErrorType.PASSWORDINVALID)
                {
                    return BadRequest("Status Code 400: Password Invalid");
                }
                else
                {
                    return BadRequest("Status Code 400: Bad Request");
                }
            }
        }

        [AllowAnonymous]
        [HttpGet("ConfirmUserName/{email}")]
        public async Task<ActionResult> ConfirmUserName(string email) {

            var result = await _userManagementService.ConfirmUserName(email);

            if (result.Success)
            {
                return Ok(result.UserName);
            }
            else
            {
                return BadRequest("Status Code 400: No Record Of Email Address");
            }            
        }
    }
}
