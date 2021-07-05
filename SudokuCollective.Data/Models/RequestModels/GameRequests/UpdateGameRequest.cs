using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Core.Models;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Validation.Attributes;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdateGameRequest : IUpdateGameRequest
    {
        [Required, GuidRegex(ErrorMessage = "License must be a valid Guid in the form of abcdefgh-1234-abcd-1234-abcdefghijkl")]
        public string License { get; set; }
        [Required]
        public int RequestorId { get; set; }
        [Required]
        public int AppId { get; set; }
        [PaginatorValidated]
        public IPaginator Paginator { get; set; }
        [Required]
        public int GameId { get; set; }
        [SudokuCellsValidated(81)]
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
