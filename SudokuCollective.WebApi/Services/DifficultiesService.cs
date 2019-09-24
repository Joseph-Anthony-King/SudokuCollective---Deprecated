using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Models;
using SudokuCollective.Models.Enums;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests;
using SudokuCollective.WebApi.Models.TaskModels.DifficultyRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class DifficultiesService : IDifficultiesService {

        private readonly ApplicationDbContext _context;

        public DifficultiesService(ApplicationDbContext context) {

            _context = context;
        }

        public async Task<DifficultyTaskResult> GetDifficulty(
            int id, bool fullRecord = true) {

            var difficultyTaskResult = new DifficultyTaskResult() {

                Result = true,
                Difficulty = new Difficulty() {

                    Id = 0,
                    Name = string.Empty,
                    DifficultyLevel = DifficultyLevel.NULL,
                    Matrices = new List<SudokuMatrix>()
                }
            };

            try {

                if (fullRecord) {

                    var difficulty = await _context.Difficulties
                        .Include(d => d.Matrices)
                        .SingleOrDefaultAsync(d => d.Id == id);

                    if (difficulty == null) {

                        difficulty = new Difficulty {

                            Id = 0,
                            Name = string.Empty,
                            DifficultyLevel = DifficultyLevel.NULL,
                            Matrices = new List<SudokuMatrix>()
                        };
                    }

                    difficulty.Matrices = await _context.SudokuMatrices
                        .Where(m => m.Difficulty.Id == difficulty.Id)
                        .ToListAsync();


                    foreach (var matrix in difficulty.Matrices)
                    {

                        matrix.SudokuCells =
                            await _context.SudokuCells
                                .Where(cell => cell.SudokuMatrix.Id == matrix.Id)
                                .OrderBy(cell => cell.Index)
                                .ToListAsync();
                    }

                    difficultyTaskResult.Result = true;
                    difficultyTaskResult.Difficulty = difficulty;

                } else {

                    var difficulty = await _context.Difficulties
                        .SingleOrDefaultAsync(d => d.Id == id);

                    if (difficulty == null) {

                        difficulty = new Difficulty {

                            Id = 0,
                            Name = string.Empty,
                            DifficultyLevel = DifficultyLevel.NULL,
                            Matrices = new List<SudokuMatrix>()
                        };
                    }

                    difficulty.Matrices = null;

                    difficultyTaskResult.Result = true;
                    difficultyTaskResult.Difficulty = difficulty;
                }

                return difficultyTaskResult;

            } catch (Exception) {

                return difficultyTaskResult;
            }
        }

        public async Task<DifficultyListTaskResult> GetDifficulties(
            bool fullRecord = true) {

            var difficultyListTaskResult = new DifficultyListTaskResult() {

                Result = false,
                Difficulties = new List<Difficulty>()
            };

            try {

                var difficulties = new List<Difficulty>();

                if (fullRecord) {

                    difficulties = await _context.Difficulties.Include(d => d.Matrices).ToListAsync();

                    difficultyListTaskResult.Result = true;
                    difficultyListTaskResult.Difficulties = difficulties;

                } else {

                    difficulties = await _context.Difficulties.ToListAsync();

                    difficultyListTaskResult.Result = true;
                    difficultyListTaskResult.Difficulties = difficulties;
                }

                return difficultyListTaskResult;

            } catch (Exception) {

                return difficultyListTaskResult;
            }            
        }

        public async Task<DifficultyTaskResult> CreateDifficulty(
            string name, DifficultyLevel difficultyLevel) {

            var difficultyTaskResult = new DifficultyTaskResult() {

                Result = false,
                Difficulty = new Difficulty() {

                    Id = 0,
                    Name = string.Empty,
                    DifficultyLevel = DifficultyLevel.NULL,
                    Matrices = new List<SudokuMatrix>()
                }
            };

            try {

                Difficulty difficulty = new Difficulty() {
                    Name = name,
                    DifficultyLevel = difficultyLevel
                };

                _context.Difficulties.Add(difficulty);
                await _context.SaveChangesAsync();

                difficultyTaskResult.Result = true;
                difficultyTaskResult.Difficulty = difficulty;

                return difficultyTaskResult;

            } catch (Exception) {

                return difficultyTaskResult;
            }
        }

        public async Task<bool> UpdateDifficulty(int id, 
            UpdateDifficultyRO updateDifficultyRO) {

            var result = false;

            try {

                if (id == updateDifficultyRO.Id) {

                    var difficulty = await _context.Difficulties
                        .Where(d => d.Id == updateDifficultyRO.Id)
                        .FirstOrDefaultAsync();

                    difficulty.Name = updateDifficultyRO.Name;
                    difficulty.DifficultyLevel = updateDifficultyRO.DifficultyLevel;

                    _context.Difficulties.Update(difficulty);

                    await _context.SaveChangesAsync();
                    result = true;
                }

                return result;

            } catch (Exception) {

                return result;
            }
        }

        public async Task<bool> DeleteDifficulty(int id) {

            var result = false;

            try {

                var difficulty = await _context.Difficulties.FindAsync(id);

                _context.Difficulties.Remove(difficulty);
                await _context.SaveChangesAsync();

                return true;

            } catch (Exception) {

                return result;
            }
        }
    }
}
