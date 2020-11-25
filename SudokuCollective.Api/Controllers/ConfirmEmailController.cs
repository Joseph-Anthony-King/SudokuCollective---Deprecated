using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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

        public ConfirmEmailController(IUsersService usersServ)
        {
            usersService = usersServ;
        }

        [AllowAnonymous]
        [HttpGet("{code}")]
        public async Task<IActionResult> Index(string code)
        {
            var result = await usersService.ConfirmEmail(code);

            if (result.Success)
            {
                var confirmEmailModel = new ConfirmEmail
                {
                    FirstName = result.FirstName,
                    AppTitle = result.AppTitle,
                    Url = result.Url,
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
