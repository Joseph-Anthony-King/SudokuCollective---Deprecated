namespace SudokuCollective.WebApi.Models.RequestModels {

    public class LicenseRequest {

        public string Name { get; set; }
        public int OwnerId { get; set; }        
        public string DevUrl { get; set; }
        public string LiveUrl { get; set; }

        public LicenseRequest() {

            Name = string.Empty;
            OwnerId = 0;
            DevUrl = string.Empty;
            LiveUrl = string.Empty;
        }
    }
}
