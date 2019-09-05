using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.RequestObjects;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase {

        private readonly IUsersService _usersService;

        public RegisterController(IUsersService usersService) {

            _usersService = usersService;
        }
        
        [AllowAnonymous]
        [HttpPost, Route("signup")]
        public async Task<ActionResult<User>> SignUp([FromBody] UserRO userRO) {
            
            var user = await _usersService.CreateUser(userRO);
            
            return CreatedAtAction("GetUser", "Users", new { id = user.Id }, user);
        }
    }
}