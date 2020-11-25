﻿using SudokuCollective.Core.Interfaces.APIModels.ResultModels.UserResults;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class ConfirmEmailResult : IConfirmEmailResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string FirstName { get; set; }
        public string AppTitle { get; set; }
        public string Url { get; set; }

        public ConfirmEmailResult() : base()
        {
            FirstName = string.Empty;
            AppTitle = string.Empty;
            Url = string.Empty;
        }
    }
}
