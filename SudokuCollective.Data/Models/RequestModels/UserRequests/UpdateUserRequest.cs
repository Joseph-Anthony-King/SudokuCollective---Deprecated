using System.ComponentModel.DataAnnotations;
using SudokuCollective.Core.Interfaces.APIModels.PageModels;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Models.PageModels;
using SudokuCollective.Data.Validation.Attributes;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdateUserRequest : IUpdateUserRequest
    {
        [Required, GuidRegex(ErrorMessage = "License must be a valid Guid in the form of abcdefgh-1234-abcd-1234-abcdefghijkl")]
        public string License { get; set; }
        [Required]
        public int RequestorId { get; set; }
        [Required]
        public int AppId { get; set; }
        [PaginatorValidated]
        public IPaginator Paginator { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        [EmailRegex(ErrorMessage = "Email address is invalid.")]
        public string Email { get; set; }

        public UpdateUserRequest() : base()
        {
            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            NickName = string.Empty;
            Email = string.Empty;

            if (Paginator == null)
            {
                Paginator = new Paginator();
            }
        }
    }
}
