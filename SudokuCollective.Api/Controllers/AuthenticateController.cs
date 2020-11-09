using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.TokenModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Enums;

namespace SudokuCollective.Api.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService authService;
        private readonly IUserManagementService userManagementService;
        private readonly string userFoundMessage;
        private readonly string userNameFoundMessage;
        private readonly string userNameMessage;
        private readonly string passwordMessage;
        private readonly string emailMessage;
        private readonly string badRequestMessage;

        public AuthenticateController(
            IAuthenticateService authServ,
            IUserManagementService userManagementServ)
        {
            authService = authServ;
            userManagementService = userManagementServ;
            userFoundMessage = "Status Code 200: User Authenticated";
            userNameFoundMessage = "Status Code 200: User Name Found";
            userNameMessage = "Status Code 400: User Name Invalid";
            passwordMessage = "Status Code 400: Password Invalid";
            emailMessage = "Status Code 400: No Record Of Email Address";
            badRequestMessage = "Status Code 400: Bad Request";
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RequestToken([FromBody] TokenRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var authenticateResult = await authService.IsAuthenticated(request);

            if (authenticateResult.Success)
            {
                var result = new AuthenticatedUserResult()
                {
                    Success = true,
                    Message = userFoundMessage,
                    User = authenticateResult.User,
                    Token = authenticateResult.Token
                };

                return Ok(result);
            }
            else
            {
                var result = await userManagementService
                    .ConfirmAuthenticationIssue(request.UserName, request.Password);

                if (result == UserAuthenticationErrorType.USERNAMEINVALID)
                {
                    return BadRequest(userNameMessage);
                }
                else if (result == UserAuthenticationErrorType.PASSWORDINVALID)
                {
                    return BadRequest(passwordMessage);
                }
                else
                {
                    return BadRequest(badRequestMessage);
                }
            }
        }

        [AllowAnonymous]
        [HttpGet("ConfirmUserName/{email}")]
        public async Task<ActionResult> ConfirmUserName(string email)
        {
            var result = await userManagementService.ConfirmUserName(email);

            if (result.Success)
            {
                result.Message = userNameFoundMessage;

                return Ok(result);
            }
            else
            {
                return BadRequest(emailMessage);
            }
        }
    }
}
