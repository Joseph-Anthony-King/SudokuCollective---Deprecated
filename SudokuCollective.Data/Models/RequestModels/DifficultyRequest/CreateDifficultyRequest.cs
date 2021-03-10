using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class CreateDifficultyRequest : ICreateDifficultyRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPaginator Paginator { get; set; }
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public DifficultyLevel DifficultyLevel { get; set; }

        public CreateDifficultyRequest() : base()
        {
            Name = string.Empty;
            DifficultyLevel = DifficultyLevel.NULL;

            if (Paginator == null)
            {
                Paginator = new Paginator();
            }
        }
    }
}
