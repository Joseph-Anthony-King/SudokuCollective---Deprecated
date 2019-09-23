namespace SudokuCollective.WebApi.Models.RequestModels.GameRequests {
    
    public class CreateGameRO {

        public int UserId { get; set; }
        public int DifficultyId { get; set; }
    }
}
