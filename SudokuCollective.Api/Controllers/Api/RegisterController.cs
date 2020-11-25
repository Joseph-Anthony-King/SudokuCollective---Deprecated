using System.IO;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Core.Models;
using SudokuCollective.Core.Interfaces.DataModels;
using SudokuCollective.Data.Messages;
using SudokuCollective.Data.Models.DataModels;
using SudokuCollective.Data.Models.RequestModels;

namespace SudokuCollective.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUsersService usersService;
        private readonly IEmailMetaData emailMetaData;
        private readonly IWebHostEnvironment hostEnvironment;

        public RegisterController(IUsersService usersServ,
            EmailMetaData metaData,
            IWebHostEnvironment environment)
        {
            usersService = usersServ;
            emailMetaData = metaData;
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
                emailtTemplatePath = Path.Combine(hostEnvironment.WebRootPath, "/Content/EmailTemplates/email-inlined.html");

                emailtTemplatePath = string.Format("../SudokuCollective.Api{0}", emailtTemplatePath);
            }
            else
            {
                emailtTemplatePath = "../../Content/EmailTemplates/email-inlined.html";
            }

            var result = await usersService.CreateUser(
                request, 
                baseUrl,
                emailtTemplatePath);

            if (result.Success)
            {
                result.Message = ControllerMessages.StatusCode201(result.Message);

                return StatusCode((int)HttpStatusCode.Created, result);
            }
            else
            {
                result.Message = ControllerMessages.StatusCode404(result.Message);

                return NotFound(result);
            }
        }
    }
}
