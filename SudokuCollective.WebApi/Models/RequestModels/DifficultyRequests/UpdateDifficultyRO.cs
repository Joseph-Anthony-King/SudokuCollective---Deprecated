using SudokuCollective.Domain.Enums;

namespace SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests {

    public class UpdateDifficultyRO : BaseRequestRO {

        public int Id { get; set; }
        public string Name { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
    }
}