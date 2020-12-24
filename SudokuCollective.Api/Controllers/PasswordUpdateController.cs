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
    public class PasswordUpdateController : Controller
    {
        private readonly IUsersService usersService;

        public PasswordUpdateController(IUsersService usersServ)
        {
            usersService = usersServ;
        }

        [AllowAnonymous]
        [HttpGet("{token}")]
        public async Task<IActionResult> Index(string token)
        {
            var result = await usersService.InitiatePasswordUpdate(token);

            if (result.Success)
            {
                var passwordUpdate = new PasswordUpdate
                {
                    Success = result.Success,
                    UserId = result.User.Id,
                    UserName = result.User.UserName,
                    AppTitle = result.App.Name,
                    Url = result.App.InDevelopment ? result.App.DevUrl : result.App.LiveUrl
                };

                return View(passwordUpdate);
            }
            else
            {
                var passwordUpdateModel = new PasswordUpdate();

                passwordUpdateModel.Success = result.Success;

                return View(passwordUpdateModel);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Result(PasswordUpdate passwordUpdate)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", passwordUpdate);
            }

            var userResut = await usersService.GetUser(passwordUpdate.UserId);

            if (userResut.Success)
            {
                var updatePasswordRequest = new UpdatePasswordRequest
                {
                    UserId = userResut.User.Id,
                    NewPassword = passwordUpdate.NewPassword
                };

                var updatePasswordResult = await usersService.UpdatePassword(updatePasswordRequest);

                passwordUpdate.NewPassword = string.Empty;

                if (updatePasswordResult.Success)
                {
                    passwordUpdate.Success = updatePasswordResult.Success;
                    passwordUpdate.Message = updatePasswordResult.Message;

                    return View(passwordUpdate);
                }
                else
                {
                    passwordUpdate.Success = updatePasswordResult.Success;
                    passwordUpdate.Message = updatePasswordResult.Message;

                    return View(passwordUpdate);
                }
            }
            else
            {
                passwordUpdate.NewPassword = string.Empty;
                passwordUpdate.Message = userResut.Message;

                return View(passwordUpdate);
            }
        }
    }
}
