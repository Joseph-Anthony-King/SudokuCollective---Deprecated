using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Domain.Enums;
using SudokuCollective.Domain.Models;
using SudokuCollective.WebApi.Models.DataModel;
using SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests;
using SudokuCollective.WebApi.Models.ResultModels;
using SudokuCollective.WebApi.Models.ResultModels.DifficultyRequests;
using SudokuCollective.WebApi.Services.Interfaces;

namespace SudokuCollective.WebApi.Services {

    public class DifficultiesService : IDifficultiesService {

        private readonly DatabaseContext _context;

        public DifficultiesService(DatabaseContext context) {

            _context = context;
        }

        public async Task<DifficultyResult> GetDifficulty(
            int id, bool fullRecord = false) {

            var difficultyTaskResult = new DifficultyResult();

            try {

                if (fullRecord) {

                    var difficulty = await _context.Difficulties
                        .Include(d => d.Matrices)
                        .SingleOrDefaultAsync(d => d.Id == id);

                    if (difficulty == null) {

                        difficultyTaskResult.Message = "Difficulty not found";

                        return difficultyTaskResult;
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

                    difficultyTaskResult.Success = true;
                    difficultyTaskResult.Difficulty = difficulty;

                } else {

                    var difficulty = await _context.Difficulties
                        .SingleOrDefaultAsync(d => d.Id == id);

                    if (difficulty == null) {

                        difficultyTaskResult.Message = "Difficulty not found";

                        return difficultyTaskResult;
                    }

                    difficulty.Matrices = null;

                    difficultyTaskResult.Success = true;
                    difficultyTaskResult.Difficulty = difficulty;
                }

                return difficultyTaskResult;

            } catch (Exception e) {

                difficultyTaskResult.Message = e.Message;

                return difficultyTaskResult;
            }
        }

        public async Task<DifficultiesResult> GetDifficulties(
            bool fullRecord = false) {

            var difficultyListTaskResult = new DifficultiesResult();

            try {

                var difficulties = new List<Difficulty>();

                if (fullRecord) {

                    difficulties = await _context.Difficulties
                        .Where(d => d.Id != 1 && d.Id != 2)
                        .OrderBy(d => d.Id)
                        .Include(d => d.Matrices)
                        .ToListAsync();

                    difficultyListTaskResult.Success = true;
                    difficultyListTaskResult.Difficulties = difficulties;

                } else {

                    difficulties = await _context.Difficulties
                        .Where(d => d.Id != 1 && d.Id != 2)
                        .OrderBy(d => d.Id)
                        .ToListAsync();

                    difficultyListTaskResult.Success = true;
                    difficultyListTaskResult.Difficulties = difficulties;
                }

                return difficultyListTaskResult;

            } catch (Exception e) {

                difficultyListTaskResult.Message = e.Message;

                return difficultyListTaskResult;
            }            
        }

        public async Task<DifficultyResult> CreateDifficulty(
            string name, DifficultyLevel difficultyLevel) {

            var difficultyTaskResult = new DifficultyResult();

            try {

                Difficulty difficulty = new Difficulty() {
                    Name = name,
                    DifficultyLevel = difficultyLevel
                };

                _context.Difficulties.Add(difficulty);
                await _context.SaveChangesAsync();

                difficultyTaskResult.Success = true;
                difficultyTaskResult.Difficulty = difficulty;

                return difficultyTaskResult;

            } catch (Exception e) {

                difficultyTaskResult.Message = e.Message;

                return difficultyTaskResult;
            }
        }

        public async Task<BaseResult> UpdateDifficulty(int id, 
            UpdateDifficultyRequest updateDifficultyRO) {

            var baseTaskResult = new BaseResult();

            try {

                if (id == updateDifficultyRO.Id) {

                    var difficulty = await _context.Difficulties
                        .Where(d => d.Id == updateDifficultyRO.Id)
                        .FirstOrDefaultAsync();

                    if (difficulty == null) {

                        baseTaskResult.Message = "Difficulty not found";

                        return baseTaskResult;
                    }

                    difficulty.Name = updateDifficultyRO.Name;
                    difficulty.DifficultyLevel = updateDifficultyRO.DifficultyLevel;

                    _context.Difficulties.Update(difficulty);

                    await _context.SaveChangesAsync();
                    baseTaskResult.Success = true;
                }

                return baseTaskResult;

            } catch (Exception e) {

                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }

        public async Task<BaseResult> DeleteDifficulty(int id) {

            var baseTaskResult = new BaseResult();

            try {

                var difficulty = await _context.Difficulties.FindAsync(id);

                if (difficulty == null) {

                    baseTaskResult.Message = "Difficulty not found";

                    return baseTaskResult;
                }

                _context.Difficulties.Remove(difficulty);
                await _context.SaveChangesAsync();

                baseTaskResult.Success = true;

                return baseTaskResult; 

            } catch (Exception e) {

                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }
    }
}
