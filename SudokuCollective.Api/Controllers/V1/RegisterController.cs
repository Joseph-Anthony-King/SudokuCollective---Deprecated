using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Data.Models.TokenModels;

namespace SudokuCollective.Api.V1.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IAuthenticateService authService;
        private readonly IWebHostEnvironment hostEnvironment;

        public RegisterController(
            IUsersService usersServ,
            IAuthenticateService authServ,
            IWebHostEnvironment environment)
        {
            usersService = usersServ;
            authService = authServ;
            hostEnvironment = environment;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> SignUp(
            [FromBody] RegisterRequest request)
        {
            string baseUrl;

            if (Request != null)
            {
                baseUrl = Request.Host.ToString();
            }
            else
            {
                baseUrl = "https://SudokuCollective.com";
            }

            string emailtTemplatePath;

            if (!string.IsNullOrEmpty(hostEnvironment.WebRootPath))
            {
                emailtTemplatePath = Path.Combine(hostEnvironment.WebRootPath, "/Content/EmailTemplates/create-email-inlined.html");

                emailtTemplatePath = string.Format("../SudokuCollective.Api{0}", emailtTemplatePath);
            }
            else
            {
                emailtTemplatePath = "../../Content/EmailTemplates/create-email-inlined.html";
            }

            var result = await usersService.CreateUser(
                request, 
                baseUrl,
                emailtTemplatePath);

            if (result.Success)
            {
                var tokenRequest = new TokenRequest
                {
                    UserName = request.UserName,
                    Password = request.Password,
                    License = request.License
                };

                var authenticateResult = await authService.IsAuthenticated(tokenRequest);

                if (authenticateResult.Success)
                {
                    result.Message = ControllerMessages.StatusCode201(result.Message);
                    result.Token = authenticateResult.Token;

                    return StatusCode((int)HttpStatusCode.Created, result);
                }
                else
                {
                    result.Message = ControllerMessages.StatusCode404(authenticateResult.Message);
                    result.User = new User();

                    return NotFound(result);
                }
            }
            else
            {
                result.Message = ControllerMessages.StatusCode404(result.Message);

                return NotFound(result);
            }
        }

        [AllowAnonymous]
        [HttpPut("ResendEmailConfirmation")]
        public async Task<ActionResult> ResendEmailConfirmation([FromBody] BaseRequest request)
        {
            string baseUrl;

            if (Request != null)
            {
                baseUrl = Request.Host.ToString();
            }
            else
            {
                baseUrl = "https://SudokuCollective.com";
            }

            string emailtTemplatePath;

            if (!string.IsNullOrEmpty(hostEnvironment.WebRootPath))
            {
                emailtTemplatePath = Path.Combine(hostEnvironment.WebRootPath, "/Content/EmailTemplates/create-email-inlined.html");

                emailtTemplatePath = string.Format("../SudokuCollective.Api{0}", emailtTemplatePath);
            }
            else
            {
                emailtTemplatePath = "../../Content/EmailTemplates/create-email-inlined.html";
            }

            var result = await usersService.ResendEmailConfirmation(
                request.RequestorId,
                request.AppId,
                baseUrl,
                emailtTemplatePath);

            if (result.Success)
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
    }
}
