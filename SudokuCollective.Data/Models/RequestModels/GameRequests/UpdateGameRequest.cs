using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdateGameRequest : IUpdateGameRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPageListModel PageListModel { get; set; }
        public int GameId { get; set; }
        public List<ISudokuCell> SudokuCells { get; set; }

        public UpdateGameRequest() : base()
        {
            GameId = 0;
            SudokuCells = new List<ISudokuCell>();
        }
    }
}
