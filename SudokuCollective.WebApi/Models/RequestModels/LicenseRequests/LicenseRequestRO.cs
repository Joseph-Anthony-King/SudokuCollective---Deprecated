namespace SudokuCollective.WebApi.Models.RequestModels {

    public class LicenseRequestRO {

        public string Name { get; set; }
        public int OwnerId { get; set; }        
        public string DevUrl { get; set; }
        public string LiveUrl { get; set; }
    }
}
