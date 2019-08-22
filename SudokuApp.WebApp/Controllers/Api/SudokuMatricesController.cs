using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Models;
using SudokuApp.WebApp.Models.DataModel;

namespace SudokuApp.WebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SudokuMatricesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SudokuMatricesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/SudokuMatrices
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SudokuMatrix>>> GetSudokuMatrix_1()
        {
            return await _context.SudokuMatrices.ToListAsync();
        }

        // GET: api/SudokuMatrices/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SudokuMatrix>> GetSudokuMatrix(int id)
        {
            var sudokuMatrix = await _context.SudokuMatrices.FindAsync(id);

            if (sudokuMatrix == null)
            {
                return NotFound();
            }

            return sudokuMatrix;
        }

        // PUT: api/SudokuMatrices/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSudokuMatrix(int id, SudokuMatrix sudokuMatrix)
        {
            if (id != sudokuMatrix.Id)
            {
                return BadRequest();
            }

            _context.Entry(sudokuMatrix).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SudokuMatrixExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/SudokuMatrices
        [HttpPost]
        public async Task<ActionResult<SudokuMatrix>> PostSudokuMatrix(SudokuMatrix sudokuMatrix)
        {
            _context.SudokuMatrices.Add(sudokuMatrix);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSudokuMatrix", new { id = sudokuMatrix.Id }, sudokuMatrix);
        }

        // DELETE: api/SudokuMatrices/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<SudokuMatrix>> DeleteSudokuMatrix(int id)
        {
            var sudokuMatrix = await _context.SudokuMatrices.FindAsync(id);
            if (sudokuMatrix == null)
            {
                return NotFound();
            }

            _context.SudokuMatrices.Remove(sudokuMatrix);
            await _context.SaveChangesAsync();

            return sudokuMatrix;
        }

        private bool SudokuMatrixExists(int id)
        {
            return _context.SudokuMatrices.Any(e => e.Id == id);
        }
    }
}
