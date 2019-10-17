namespace SudokuCollective.WebApi.Models.RequestModels.GameRequests {

    public class GetMyGameRequest : BaseRequest {

        public int UserId { get; set; }

        public GetMyGameRequest() : base() {

            UserId = 0;
        }
    }
}
