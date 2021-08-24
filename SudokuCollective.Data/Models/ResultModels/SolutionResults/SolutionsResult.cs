﻿using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class SolutionsResult : ISolutionsResult
    {
        public bool IsSuccess { get; set; }
        public bool FromCache { get; set; }
        public string Message { get; set; }
        public List<ISudokuSolution> Solutions { get; set; }

        public SolutionsResult() : base()
        {
            IsSuccess = false;
            FromCache = false;
            Message = string.Empty;
            Solutions = new List<ISudokuSolution>();
        }
    }
}
