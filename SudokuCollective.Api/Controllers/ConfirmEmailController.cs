using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Api.Models;
using SudokuCollective.Core.Interfaces.Services;

namespace SudokuCollective.Api.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class ConfirmEmailController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IWebHostEnvironment hostEnvironment;

        public ConfirmEmailController(
            IUsersService usersServ,
            IWebHostEnvironment environment)
        {
            usersService = usersServ;
            hostEnvironment = environment;
        }

        [AllowAnonymous]
        [HttpGet("{token}")]
        public async Task<IActionResult> Index(string token)
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
                emailtTemplatePath = Path.Combine(hostEnvironment.WebRootPath, "/Content/EmailTemplates/confirm-new-email-inlined.html");

                emailtTemplatePath = string.Format("../SudokuCollective.Api{0}", emailtTemplatePath);
            }
            else
            {
                emailtTemplatePath = "../../Content/EmailTemplates/confirm-new-email-inlined.html";
            }

            var result = await usersService.ConfirmEmail(token, baseUrl, emailtTemplatePath);

            if (result.Success)
            {
                var confirmEmailModel = new ConfirmEmail
                {
                    UserName = result.UserName,
                    AppTitle = result.AppTitle,
                    Url = result.Url,
                    IsUpdate = result.IsUpdate != null && (bool)result.IsUpdate,
                    NewEmailAddressConfirmed = result.NewEmailAddressConfirmed != null && (bool)result.NewEmailAddressConfirmed,
                    Success = result.Success
                };

                return View(confirmEmailModel);
            }
            else
            {
                var confirmEmailModel = new ConfirmEmail();

                confirmEmailModel.Success = result.Success;

                return View(confirmEmailModel);
            }
        }
    }
}
