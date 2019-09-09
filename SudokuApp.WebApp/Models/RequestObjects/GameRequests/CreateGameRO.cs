namespace SudokuApp.WebApp.Models.RequestObjects.GameRequests {
    
    public class CreateGameRO {

        public int UserId { get; set; }
        public int DifficultyId { get; set; }
    }
}
