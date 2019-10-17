using System.Collections.Generic;
using SudokuCollective.Domain.Models;

namespace SudokuCollective.WebApi.Models.ResultModels.AppRequests {

    public class AppsResult : BaseResult {
        
        public IEnumerable<App> Apps { get; set; }

        public AppsResult() : base() {

            Apps = new List<App>();
        }
    }
}
