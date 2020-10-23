using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class DifficultiesResult : IDifficultiesResult
    {
        public bool Success { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public string Message { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
        public List<IDifficulty> Difficulties { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public DifficultiesResult() : base()
        {
            Difficulties = new List<IDifficulty>();
        }
    }
}
