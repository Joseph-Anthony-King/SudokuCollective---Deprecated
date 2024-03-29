﻿using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class AnnonymousGameResult : IAnnonymousGameResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFromCache { get; set; }
        public string Message { get; set; }
        public List<List<int>> SudokuMatrix { get; set; }

        public AnnonymousGameResult()
        {
            IsSuccess = false;
            IsFromCache = false;
            Message = string.Empty;
            SudokuMatrix = new List<List<int>>();
        }
    }
}
