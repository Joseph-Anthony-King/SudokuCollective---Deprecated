using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class AppsResult : IAppsResult
    {
        public bool IsSuccess { get; set; }
        public bool IsFromCache { get; set; }
        public string Message { get; set; }
        public List<IApp> Apps { get; set; }

        public AppsResult() : base()
        {
            IsSuccess = false;
            IsFromCache = false;
            Message = string.Empty;
            Apps = new List<IApp>();
        }
    }
}
