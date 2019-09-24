namespace SudokuCollective.WebApi.Models.RequestModels.GameRequests {
    
    public class CreateGameRO : BaseRequestRO {

        public int UserId { get; set; }
        public int DifficultyId { get; set; }
    }
}
