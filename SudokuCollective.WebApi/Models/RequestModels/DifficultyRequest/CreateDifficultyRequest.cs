using SudokuCollective.Domain.Enums;

namespace SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests {

    public class CreateDifficultyRequest : BaseRequest {

        public string Name { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }

        public CreateDifficultyRequest() : base() {

            Name = string.Empty;
            DifficultyLevel = DifficultyLevel.NULL;
        }
    }
}
