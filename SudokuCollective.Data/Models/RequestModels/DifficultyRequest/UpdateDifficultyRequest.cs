using System.ComponentModel.DataAnnotations;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Validation.Attributes;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdateDifficultyRequest : IUpdateDifficultyRequest
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
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string DisplayName { get; set; }

        public UpdateDifficultyRequest() : base()
        {
            Id = 0;
            Name = string.Empty;
            DisplayName = string.Empty;

            if (Paginator == null)
            {
                Paginator = new Paginator();
            }
        }
    }
}
