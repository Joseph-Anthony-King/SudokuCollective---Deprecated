namespace SudokuApp.WebApi.Models.RequestObjects.GameRequests {

    public class GetMyGameRO {

        public int UserId { get; set; }
        public int GameId { get; set; }
    }
}