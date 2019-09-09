using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SudokuApp.Models;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase {

        private readonly IUsersService _userService;

        public UsersController(IUsersService userService) {

            _userService = userService;
        }

        // GET: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(
            int id, [FromQuery] bool fullRecord = true) {

            var user = await _userService.GetUser(id, fullRecord);

            if (string.IsNullOrEmpty(user.Value.UserName)) {

                return BadRequest();

            } else {

                return user;
            }
        }

        // GET: api/Users
        [Authorize(Roles = "SUPERUSER, ADMIN")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers(
            [FromQuery] bool fullRecord = true) {

            return await _userService.GetUsers(fullRecord);
        }

        // PUT: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(
            int id, [FromBody] User user) {

            if (id != user.Id) {

                return BadRequest();
            }
            
            await _userService.UpdateUser(id, user);

            return NoContent();
        }

        // DELETE: api/Users/5
        [Authorize(Roles = "SUPERUSER, ADMIN, USER")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(int id) {

           var user = await _userService.DeleteUser(id);

           return user;
        }
    }
}
