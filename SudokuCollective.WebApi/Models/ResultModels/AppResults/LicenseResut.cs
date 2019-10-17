namespace SudokuCollective.WebApi.Models.ResultModels.AppRequests {

    public class LicenseResut : BaseResult {
        
        public string License { get; set; }

        public LicenseResut() : base() {

            License = string.Empty;
        }
    }
}
