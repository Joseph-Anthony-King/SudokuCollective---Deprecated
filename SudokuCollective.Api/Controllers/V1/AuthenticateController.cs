using System;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Enums;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.TokenModels;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Data.Models.RequestModels;

namespace SudokuCollective.Api.V1.Controllers
{
    [Authorize]
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AuthenticateController : ControllerBase
    {
        private readonly IAuthenticateService authService;
        private readonly IUserManagementService userManagementService;

        public AuthenticateController(
            IAuthenticateService authServ,
            IUserManagementService userManagementServ)
        {
            authService = authServ;
            userManagementService = userManagementServ;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> RequestToken([FromBody] TokenRequest request)
        {
            try
            {

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var authenticateResult = await authService.IsAuthenticated(request);

                if (authenticateResult.IsSuccess)
                {
                    var result = new AuthenticatedUserResult()
                    {
                        IsSuccess = true,
                        Message = ControllerMessages.StatusCode200(authenticateResult.Message),
                        User = authenticateResult.User,
                        Token = authenticateResult.Token
                    };

                    return Ok(result);
                }
                else if (authenticateResult.Message.Equals(AppsMessages.AppDeactivatedMessage))
                {
                    return NotFound(ControllerMessages.StatusCode404(authenticateResult.Message));
                }
                else if (authenticateResult.Message.Equals(AppsMessages.UserIsNotARegisteredUserOfThisAppMessage))
                {
                    return NotFound(ControllerMessages.StatusCode404(authenticateResult.Message));
                }
                else
                {
                    var confirmAuthenticationIssueResponse = await userManagementService
                        .ConfirmAuthenticationIssue(
                            request.UserName,
                            request.Password,
                            request.License);

                    var result = new BaseResult
                    {
                        IsSuccess = false
                    };

                    if (confirmAuthenticationIssueResponse == UserAuthenticationErrorType.USERNAMEINVALID)
                    {
                        result.Message = ControllerMessages.StatusCode404("No User Has This User Name");
                        return NotFound(result);
                    }
                    else if (confirmAuthenticationIssueResponse == UserAuthenticationErrorType.PASSWORDINVALID)
                    {
                        result.Message = ControllerMessages.StatusCode404("Password Invalid");
                        return NotFound(result);
                    }
                    else
                    {
                        return NotFound(ControllerMessages.StatusCode404("Bad Request"));
                    }
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    IsSuccess = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }

        [AllowAnonymous]
        [HttpPost("ConfirmUserName")]
        public async Task<ActionResult> ConfirmUserName([FromBody] ConfirmUserNameRequest request)
        {
            try
            {
                var result = await userManagementService.ConfirmUserName(
                    request.Email,
                    request.License);

                if (result.IsSuccess)
                {
                    result.Message = ControllerMessages.StatusCode200(result.Message);

                    return Ok(result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(result.Message);
                    return NotFound(result);
                }
            }
            catch (Exception e)
            {
                var result = new BaseResult
                {
                    IsSuccess = false,
                    Message = ControllerMessages.StatusCode500(e.Message)
                };

                return StatusCode((int)HttpStatusCode.InternalServerError, result);
            }
        }
    }
}
