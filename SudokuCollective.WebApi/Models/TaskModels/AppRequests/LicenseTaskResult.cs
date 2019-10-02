namespace SudokuCollective.WebApi.Models.TaskModels.AppRequests {

    public class LicenseTaskResult : BaseTaskResult {
        
        public string License { get; set; }

        public LicenseTaskResult() : base() {

            License = string.Empty;
        }
    }
}
