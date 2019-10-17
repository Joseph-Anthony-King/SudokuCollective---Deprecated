namespace SudokuCollective.WebApi.Models.RequestModels.AppRequests {

    public class AppRequest : BaseRequest {

        public string Name { get; set; }
        public string DevUrl { get; set; }
        public string LiveUrl { get; set; }

        public AppRequest() : base() {

            Name = string.Empty;
            DevUrl = string.Empty;
            LiveUrl = string.Empty;
        }
    }
}
