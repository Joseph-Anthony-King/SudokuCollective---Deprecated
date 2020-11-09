using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models.RequestModels;
using SudokuCollective.Core.Models;
using System.Net;

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
            [FromBody] RegisterRequest registerRO)
        {
            var result = await usersService.CreateUser(registerRO);

            if (result.Success)
            {
                result.Message = "Status Code 201: User Created";

                return StatusCode((int)HttpStatusCode.Created, result);
            }
            else
            {
                return NotFound(result.Message);
            }
        }
    }
}
