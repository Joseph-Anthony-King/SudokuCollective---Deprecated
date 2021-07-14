using System.ComponentModel.DataAnnotations;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels.GameRequests;

namespace SudokuCollective.Data.Models.RequestModels.GameRequests
{
    public class AnnonymousGameRequest : IAnnonymousGameRequest
    {
        [Required]
        public DifficultyLevel DifficultyLevel { get; set; }
    }
}
