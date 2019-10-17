﻿using System.Collections.Generic;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.WebApi.Models.ResultModels.SolutionRequests {

    public class SolutionsResult : BaseResult {
        
        public IEnumerable<SudokuSolution> Solutions { get; set; }

        public SolutionsResult() : base() {

            Solutions = new List<SudokuSolution>();
        }
    }
}