using System.Collections.Generic;
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

        public async Task<ActionResult<Difficulty>> GetDifficulty(int id) {

            var difficulty = await _context.Difficulties.SingleOrDefaultAsync(d => d.Id == id);

            if (difficulty == null) {

                return new Difficulty {
                    
                    Name = string.Empty,
                    DifficultyLevel = DifficultyLevel.NULL,
                    Matrices = new List<SudokuMatrix>()
                };
            }

            return difficulty;
        }

        public async Task<ActionResult<IEnumerable<Difficulty>>> GetDifficulties() {

            return await _context.Difficulties.Include(d => d.Matrices).ToListAsync();
        }

        public async Task<Difficulty> CreateDifficulty(string name, DifficultyLevel difficultyLevel) {

            Difficulty difficulty = new Difficulty() { Name = name, DifficultyLevel = difficultyLevel };

            _context.Difficulties.Add(difficulty);
            await _context.SaveChangesAsync();

            return difficulty;
        }

        public async Task UpdateDifficulty(int id, Difficulty difficulty) {

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