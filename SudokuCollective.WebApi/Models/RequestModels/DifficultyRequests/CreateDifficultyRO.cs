using SudokuCollective.Domain.Enums;

namespace SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests {

    public class CreateDifficultyRO : BaseRequestRO {

        public string Name { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }
    }
}