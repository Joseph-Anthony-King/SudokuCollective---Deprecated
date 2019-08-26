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
    public class DifficultiesController : ControllerBase {

        private readonly ApplicationDbContext _context;

        public DifficultiesController(ApplicationDbContext context) {

            _context = context;
        }

        // GET: api/Difficulties
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties() {

            return await _context.Difficulties.ToListAsync();
        }

        // GET: api/Difficulties/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Difficulty>> GetDifficulty(int id) {

            var difficulty = await _context.Difficulties.FindAsync(id);

            if (difficulty == null)
            {
                return NotFound();
            }

            return difficulty;
        }

        // PUT: api/Difficulties/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDifficulty(int id, Difficulty difficulty) {

            if (id != difficulty.Id) {

                return BadRequest();
            }

            _context.Entry(difficulty).State = EntityState.Modified;

            try {

                await _context.SaveChangesAsync();

            } catch (DbUpdateConcurrencyException) {

                if (!DifficultyExists(id)) {

                    return NotFound();

                } else {

                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Difficulties
        [HttpPost]
        public async Task<ActionResult<Difficulty>> PostDifficulty(Difficulty difficulty) {

            _context.Difficulties.Add(difficulty);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetDifficulty", new { id = difficulty.Id }, difficulty);
        }

        // DELETE: api/Difficulties/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Difficulty>> DeleteDifficulty(int id) {

            var difficulty = await _context.Difficulties.FindAsync(id);

            if (difficulty == null) {

                return NotFound();
            }

            _context.Difficulties.Remove(difficulty);
            await _context.SaveChangesAsync();

            return difficulty;
        }

        private bool DifficultyExists(int id) {

            return _context.Difficulties.Any(e => e.Id == id);
        }
    }
}
