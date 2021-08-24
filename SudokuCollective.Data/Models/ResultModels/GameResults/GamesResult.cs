﻿using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class GamesResult : IGamesResult
    {
        public bool IsSuccess { get; set; }
        public bool FromCache { get; set; }
        public string Message { get; set; }
        public List<IGame> Games { get; set; }

        public GamesResult() : base()
        {
            IsSuccess = false;
            FromCache = false;
            Message = string.Empty;
            Games = new List<IGame>();
        }
    }
}
