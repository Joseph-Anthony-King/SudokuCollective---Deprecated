using System.Collections.Generic;
using SudokuCollective.Domain;

namespace SudokuCollective.WebApi.Models.TaskModels.AppRequests {

    public class AppListTaskResult : BaseTaskResult {
        
        public IEnumerable<App> Apps { get; set; }

        public AppListTaskResult() : base() {

            Apps = new List<App>();
        }
    }
}
