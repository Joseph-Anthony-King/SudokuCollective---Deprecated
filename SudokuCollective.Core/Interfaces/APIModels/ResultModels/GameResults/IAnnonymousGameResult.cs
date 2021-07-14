using System.Collections.Generic;

namespace SudokuCollective.Core.Interfaces.APIModels.ResultModels
{
    public interface IAnnonymousGameResult : IBaseResult
    {
        public List<List<int>> SudokuMatrix { get; set; }
    }
}
