namespace SudokuCollective.WebApi.Models.ResultModels.AppRequests {

    public class LicenseResult : BaseResult {
        
        public string License { get; set; }

        public LicenseResult() : base() {

            License = string.Empty;
        }
    }
}
