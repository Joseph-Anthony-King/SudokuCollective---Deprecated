namespace SudokuCollective.WebApi.Models.RequestModels.AppRequests {

    public class UpdateAppRO : BaseRequestRO {

        public string Name { get; set; }
        public string DevUrl { get; set; }
        public string LiveUrl { get; set; }
    }
}
