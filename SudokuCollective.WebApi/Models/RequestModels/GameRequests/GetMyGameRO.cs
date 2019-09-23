namespace SudokuCollective.WebApi.Models.RequestModels.GameRequests {

    public class GetMyGameRO {

        public int UserId { get; set; }
        public int GameId { get; set; }
    }
}