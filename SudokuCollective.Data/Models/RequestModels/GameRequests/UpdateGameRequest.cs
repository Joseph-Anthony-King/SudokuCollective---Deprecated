using System.Collections.Generic;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models.PageModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdateGameRequest : IUpdateGameRequest
    {
        public string License { get; set; }
        public int RequestorId { get; set; }
        public int AppId { get; set; }
        public IPaginator Paginator { get; set; }
        public int GameId { get; set; }
        public List<SudokuCell> SudokuCells { get; set; }

        public UpdateGameRequest() : base()
        {
            GameId = 0;
            SudokuCells = new List<SudokuCell>();

            if (Paginator == null)
            {
                Paginator = new Paginator();
            }
        }
    }
}
