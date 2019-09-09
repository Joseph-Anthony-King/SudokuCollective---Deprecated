using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SudokuApp.Models;
using SudokuApp.Models.Enums;
using SudokuApp.WebApp.Models.DataModel;
using SudokuApp.WebApp.Services.Interfaces;

namespace SudokuApp.WebApp.Services {

    public class DifficultiesService : IDifficultiesService {

        private readonly ApplicationDbContext _context;

        public DifficultiesService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<ActionResult<Difficulty>> GetDifficulty(
            int id, bool fullRecord = true) {

            var difficulty = new Difficulty();

            if (fullRecord) {
                
                difficulty = await _context.Difficulties
                    .Include(d => d.Matrices)
                    .SingleOrDefaultAsync(d => d.Id == id);

                if (difficulty == null) {

                    return new Difficulty {
                        
                        Name = string.Empty,
                        DifficultyLevel = DifficultyLevel.NULL,
                        Matrices = new List<SudokuMatrix>()
                    };
                }

                difficulty.Matrices = await _context.SudokuMatrices
                    .Where(m => m.Difficulty.Id == difficulty.Id)
                    .ToListAsync();

                
                foreach (var matrix in difficulty.Matrices) {

                    matrix.SudokuCells = 
                        await _context.SudokuCells
                            .Where(cell => cell.SudokuMatrix.Id == matrix.Id)
                            .OrderBy(cell => cell.Index)
                            .ToListAsync();
                }

            } else {
                
                difficulty = await _context.Difficulties
                    .SingleOrDefaultAsync(d => d.Id == id);

                if (difficulty == null) {

                    return new Difficulty {
                        
                        Name = string.Empty,
                        DifficultyLevel = DifficultyLevel.NULL,
                        Matrices = new List<SudokuMatrix>()
                    };
                }

                difficulty.Matrices = null;
            }

            return difficulty;
        }

        public async Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties(
            bool fullRecord = true) {

            var difficulties = new List<Difficulty>();

            if (fullRecord) {
                
                return await _context.Difficulties.Include(d => d.Matrices).ToListAsync();

            } else {

                return await _context.Difficulties.ToListAsync();
            }

            
        }

        public async Task<Difficulty> CreateDifficulty(
            string name, DifficultyLevel difficultyLevel) {

            Difficulty difficulty = new Difficulty() { Name = name, DifficultyLevel = difficultyLevel };

            _context.Difficulties.Add(difficulty);
            await _context.SaveChangesAsync();

            return difficulty;
        }

        public async Task UpdateDifficulty(int id, 
            Difficulty difficulty) {

            if (id == difficulty.Id) {

                _context.Entry(difficulty).State = EntityState.Modified;
                
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Difficulty> DeleteDifficulty(int id) {

            var difficulty = await _context.Difficulties.FindAsync(id);

            if (difficulty == null) {

                return new Difficulty {
                    
                    Name = string.Empty,
                    DifficultyLevel = DifficultyLevel.NULL,
                    Matrices = new List<SudokuMatrix>()
                };
            }

            _context.Difficulties.Remove(difficulty);
            await _context.SaveChangesAsync();

            return difficulty;
        }
    }
}
