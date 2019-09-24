using System.Collections.Generic;
using SudokuCollective.Models;

namespace SudokuCollective.WebApi.Models.TaskModels.AppRequests {

    public class AppListTaskResult {

        public bool Result { get; set; }
        public IEnumerable<App> Apps { get; set; }
    }
}
