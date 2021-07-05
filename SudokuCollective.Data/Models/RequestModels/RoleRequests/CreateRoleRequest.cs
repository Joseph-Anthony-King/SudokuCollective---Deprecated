using System.ComponentModel.DataAnnotations;
using SudokuCollective.Core.Enums;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Validation.Attributes;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class CreateRoleRequest : ICreateRoleRequest
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
        public string Name { get; set; }
        [Required]
        public RoleLevel RoleLevel { get; set; }

        public CreateRoleRequest() : base()
        {
            Name = string.Empty;
            RoleLevel = RoleLevel.NULL;

            if (Paginator == null)
            {
                Paginator = new Paginator();
            }
        }
    }
}
