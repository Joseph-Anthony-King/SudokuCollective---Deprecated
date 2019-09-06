using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.DataModel;

namespace SudokuApp.WebApp.Controllers {

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase {

        private readonly ApplicationDbContext _context;

        public RolesController(ApplicationDbContext context) {

            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles() {

            return await _context.Roles.Include(u => u.Users).ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id) {

            var role = await _context.Roles.Include(u => u.Users).SingleOrDefaultAsync(u => u.Id == id);

            if (role == null)
            {
                return NotFound();
            }

            return role;
        }

        // PUT: api/Roles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, Role role) {

            if (id != role.Id) {

                return BadRequest();
            }

            _context.Entry(role).State = EntityState.Modified;

            try {
                await _context.SaveChangesAsync();

            } catch (DbUpdateConcurrencyException) {

                if (!RoleExists(id)) {

                    return NotFound();

                } else {

                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Roles
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(Role role) {

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetRole", new { id = role.Id }, role);
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Role>> DeleteRole(int id) {

            var role = await _context.Roles.FindAsync(id);

            if (role == null) {

                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();

            return role;
        }

        private bool RoleExists(int id) {

            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
