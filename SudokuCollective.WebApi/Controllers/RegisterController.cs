using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.RequestModels.RegisterRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase {

        private readonly IUsersService _usersService;

        public RegisterController(IUsersService usersService) {

            _usersService = usersService;
        }
        
        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult<User>> SignUp(
            [FromBody] RegisterRequest registerRO,
            [FromQuery] bool addAdmin = false) {
            
            var result = await _usersService.CreateUser(registerRO, addAdmin);

            if (result.Success) {

                return CreatedAtAction(
                    "GetUser", 
                    "Users", 
                    new { id = result.User.Id }, 
                    result.User);

            } else {

                return NotFound(result.Message);
            }
        }
    }
}
