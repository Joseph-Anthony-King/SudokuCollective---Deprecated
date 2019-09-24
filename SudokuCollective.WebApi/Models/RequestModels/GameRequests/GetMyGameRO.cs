namespace SudokuCollective.WebApi.Models.RequestModels.GameRequests {

    public class GetMyGameRO : BaseRequestRO {

        public int UserId { get; set; }
        public int GameId { get; set; }
    }
}