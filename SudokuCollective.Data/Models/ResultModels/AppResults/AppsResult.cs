using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.ResultModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class AppsResult : IAppsResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<IApp> Apps { get; set; }

        public AppsResult() : base()
        {
            Apps = new List<IApp>();
        }
    }
}
