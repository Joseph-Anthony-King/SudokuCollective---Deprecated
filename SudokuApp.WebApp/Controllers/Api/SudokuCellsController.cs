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
    public class SudokuCellsController : ControllerBase {

        private readonly ApplicationDbContext _context;

        public SudokuCellsController(ApplicationDbContext context) {

            _context = context;
        }

        // GET: api/SudokuCells
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SudokuCell>>> GetSudokuCells() {

            return await _context.SudokuCells.ToListAsync();
        }

        // GET: api/SudokuCells/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SudokuCell>> GetSudokuCell(int id) {

            var sudokuCell = await _context.SudokuCells.FindAsync(id);

            if (sudokuCell == null) {

                return NotFound();
            }

            return sudokuCell;
        }

        // PUT: api/SudokuCells/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSudokuCell(int id, SudokuCell sudokuCell) {

            if (id != sudokuCell.Id) {

                return BadRequest();
            }

            _context.Entry(sudokuCell).State = EntityState.Modified;

            try {

                await _context.SaveChangesAsync();

            } catch (DbUpdateConcurrencyException) {

                if (!SudokuCellExists(id)) {

                    return NotFound();

                } else {

                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SudokuCells
        [HttpPost]
        public async Task<ActionResult<SudokuCell>> PostSudokuCell(SudokuCell sudokuCell) {

            _context.SudokuCells.Add(sudokuCell);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSudokuCell", new { id = sudokuCell.Id }, sudokuCell);
        }

        // DELETE: api/SudokuCells/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SudokuCell>> DeleteSudokuCell(int id) {

            var sudokuCell = await _context.SudokuCells.FindAsync(id);

            if (sudokuCell == null) {

                return NotFound();
            }

            _context.SudokuCells.Remove(sudokuCell);
            await _context.SaveChangesAsync();

            return sudokuCell;
        }

        private bool SudokuCellExists(int id) {

            return _context.SudokuCells.Any(e => e.Id == id);
        }
    }
}
