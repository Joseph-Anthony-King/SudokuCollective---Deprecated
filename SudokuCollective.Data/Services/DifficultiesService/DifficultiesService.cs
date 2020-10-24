using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;
using SudokuCollective.Core.Interfaces.Services;
using SudokuCollective.Data.Models;
using SudokuCollective.Data.Models.ResultModels;
using SudokuCollective.Core.Models;

namespace SudokuCollective.Data.Services
{
    public class DifficultiesService : IDifficultiesService
    {
        private readonly DatabaseContext _context;

        public DifficultiesService(DatabaseContext context)
        {
            _context = context;
        }

        public async Task<IDifficultyResult> GetDifficulty(
            int id, bool fullRecord = false)
        {

            var difficultyTaskResult = new DifficultyResult();

            try
            {

                if (fullRecord)
                {

                    var difficulty = await _context.Difficulties
                        .Include(d => d.Matrices)
                        .SingleOrDefaultAsync(d => d.Id == id);

                    if (difficulty == null)
                    {

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

                }
                else
                {

                    var difficulty = await _context.Difficulties
                        .SingleOrDefaultAsync(d => d.Id == id);

                    if (difficulty == null)
                    {

                        difficultyTaskResult.Message = "Difficulty not found";

                        return difficultyTaskResult;
                    }

                    difficulty.Matrices = null;

                    difficultyTaskResult.Success = true;
                    difficultyTaskResult.Difficulty = difficulty;
                }

                return difficultyTaskResult;

            }
            catch (Exception e)
            {

                difficultyTaskResult.Message = e.Message;

                return difficultyTaskResult;
            }
        }

        public async Task<IDifficultiesResult> GetDifficulties(
            bool fullRecord = false)
        {
            var difficultyListTaskResult = new DifficultiesResult();

            try
            {
                if (fullRecord)
                {
                    var difficulties = await _context.Difficulties
                        .Where(d => d.Id != 1 && d.Id != 2)
                        .OrderBy(d => d.Id)
                        .Include(d => d.Matrices)
                        .ToListAsync();

                    difficultyListTaskResult.Success = true;
                    difficultyListTaskResult.Difficulties = difficulties.ConvertAll(d => d as IDifficulty);
                }
                else
                {
                    var difficulties = await _context.Difficulties
                        .Where(d => d.Id != 1 && d.Id != 2)
                        .OrderBy(d => d.Id)
                        .ToListAsync();

                    difficultyListTaskResult.Success = true;
                    difficultyListTaskResult.Difficulties = difficulties.ConvertAll(d => d as IDifficulty);
                }

                return difficultyListTaskResult;

            }
            catch (Exception e)
            {
                difficultyListTaskResult.Message = e.Message;

                return difficultyListTaskResult;
            }
        }

        public async Task<IDifficultyResult> CreateDifficulty(
            string name, DifficultyLevel difficultyLevel)
        {
            var difficultyTaskResult = new DifficultyResult();

            try
            {
                Difficulty difficulty = new Difficulty()
                {
                    Name = name,
                    DifficultyLevel = difficultyLevel
                };

                _context.Difficulties.Add(difficulty);
                await _context.SaveChangesAsync();

                difficultyTaskResult.Success = true;
                difficultyTaskResult.Difficulty = difficulty;

                return difficultyTaskResult;
            }
            catch (Exception e)
            {
                difficultyTaskResult.Message = e.Message;

                return difficultyTaskResult;
            }
        }

        public async Task<IBaseResult> UpdateDifficulty(int id,
            IUpdateDifficultyRequest updateDifficultyRequest)
        {
            var baseTaskResult = new BaseResult();

            try
            {
                if (id == updateDifficultyRequest.Id)
                {
                    var difficulty = await _context.Difficulties
                        .Where(d => d.Id == updateDifficultyRequest.Id)
                        .FirstOrDefaultAsync();

                    if (difficulty == null)
                    {
                        baseTaskResult.Message = "Difficulty not found";

                        return baseTaskResult;
                    }

                    difficulty.Name = updateDifficultyRequest.Name;
                    difficulty.DifficultyLevel = updateDifficultyRequest.DifficultyLevel;

                    _context.Difficulties.Update(difficulty);

                    await _context.SaveChangesAsync();
                    baseTaskResult.Success = true;
                }

                return baseTaskResult;
            }
            catch (Exception e)
            {
                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }

        public async Task<IBaseResult> DeleteDifficulty(int id)
        {
            var baseTaskResult = new BaseResult();

            try
            {
                var difficulty = await _context.Difficulties.FindAsync(id);

                if (difficulty == null)
                {
                    baseTaskResult.Message = "Difficulty not found";

                    return baseTaskResult;
                }

                _context.Difficulties.Remove(difficulty);
                await _context.SaveChangesAsync();

                baseTaskResult.Success = true;

                return baseTaskResult;
            }
            catch (Exception e)
            {
                baseTaskResult.Message = e.Message;

                return baseTaskResult;
            }
        }
    }
}
