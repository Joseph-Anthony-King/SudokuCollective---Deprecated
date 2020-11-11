using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class CreateGameRequest : ICreateGameRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPageListModel PageListModel { get; set; }
        public int UserId { get; set; }
        public int DifficultyId { get; set; }

        public CreateGameRequest() : base()
        {
            UserId = 0;
            DifficultyId = 0;

            if (PageListModel == null)
            {
                PageListModel = new PageListModel();
            }
        }
    }
}
