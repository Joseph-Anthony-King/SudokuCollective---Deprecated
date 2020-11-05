using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Api.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly IUsersService usersService;

        public RegisterController(IUsersService usersServ)
        {
            usersService = usersServ;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> SignUp(
            [FromBody] RegisterRequest registerRO,
            [FromQuery] bool addAdmin = false)
        {
            var result = await usersService.CreateUser(registerRO, addAdmin);

            if (result.Success)
            {
                return CreatedAtAction(
                    "GetUser",
                    "Users",
                    new { id = result.User.Id },
                    result.User);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
    }
}
