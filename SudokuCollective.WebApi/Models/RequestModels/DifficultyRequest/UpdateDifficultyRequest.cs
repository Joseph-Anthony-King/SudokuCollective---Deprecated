using SudokuCollective.Domain.Enums;

namespace SudokuCollective.WebApi.Models.RequestModels.DifficultyRequests {

    public class UpdateDifficultyRequest : BaseRequest {

        public int Id { get; set; }
        public string Name { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }

        public UpdateDifficultyRequest() : base() {

            Id = 0;
            Name = string.Empty;
            DifficultyLevel = DifficultyLevel.NULL;
        }
    }
}
