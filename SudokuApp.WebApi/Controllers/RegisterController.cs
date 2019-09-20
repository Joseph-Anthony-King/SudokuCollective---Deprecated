using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApi.Models.RequestObjects.RegisterRequests;
using SudokuApp.WebApi.Services.Interfaces;

namespace SudokuApp.WebApi.Controllers {

    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase {

        private readonly IUsersService _usersService;

        public RegisterController(IUsersService usersService) {

            _usersService = usersService;
        }
        
        [AllowAnonymous]
        [HttpPost, Route("signup")]
        public async Task<ActionResult<User>> SignUp([FromBody] RegisterRO registerRO) {
            
            var user = await _usersService.CreateUser(registerRO);
            
            return CreatedAtAction("GetUser", "Users", new { id = user.Id }, user);
        }
    }
}