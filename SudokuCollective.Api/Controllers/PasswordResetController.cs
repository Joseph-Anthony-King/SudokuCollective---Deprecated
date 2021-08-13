using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Api.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;

namespace SudokuCollective.Api.Controllers
{
    [Route("[controller]")]
    [Controller]
    public class PasswordResetController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IAppsService appsService;

        public PasswordResetController(
            IUsersService usersServ,
            IAppsService appsServ)
        {
            usersService = usersServ;
            appsService = appsServ;
        }

        [AllowAnonymous]
        [HttpGet("{token}")]
        public async Task<IActionResult> Index(string token)
        {
            var licenseResult = await usersService
                .GetAppLicenseByPasswordToken(token);

            var result = await usersService
                .InitiatePasswordReset(
                    token,
                    licenseResult.License);

            if (result.Success)
            {
                var passwordReset = new PasswordReset
                {
                    Success = result.Success,
                    UserId = result.User.Id,
                    UserName = result.User.UserName,
                    AppTitle = result.App.Name,
                    AppId = result.App.Id,
                    Url = result.App.InDevelopment ? result.App.DevUrl : result.App.LiveUrl
                };

                return View(passwordReset);
            }
            else
            {
                var passwordReset = new PasswordReset
                {
                    Success = result.Success
                };

                return View(passwordReset);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Result(
            PasswordReset passwordReset)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", passwordReset);
            }

            var app = (await appsService.Get(passwordReset.AppId)).App;
            app.License = (await appsService.GetLicense(app.Id)).License;

            var userResut = await usersService.Get(
                passwordReset.UserId, 
                app.License);

            if (userResut.Success)
            {
                var updatePasswordRequest = new UpdatePasswordRequest
                {
                    UserId = userResut.User.Id,
                    NewPassword = passwordReset.NewPassword
                };

                var updatePasswordResult = await usersService.UpdatePassword(updatePasswordRequest, app.License);

                passwordReset.NewPassword = string.Empty;

                if (updatePasswordResult.Success)
                {
                    passwordReset.Success = updatePasswordResult.Success;
                    passwordReset.Message = updatePasswordResult.Message;

                    return View(passwordReset);
                }
                else
                {
                    passwordReset.Success = updatePasswordResult.Success;
                    passwordReset.Message = updatePasswordResult.Message;

                    return View(passwordReset);
                }
            }
            else
            {
                passwordReset.NewPassword = string.Empty;
                passwordReset.Message = userResut.Message;

                return View(passwordReset);
            }
        }
    }
}
