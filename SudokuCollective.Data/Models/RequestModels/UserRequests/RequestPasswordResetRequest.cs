using System.ComponentModel.DataAnnotations;
using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using SudokuCollective.Data.Validation.Attributes;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class RequestPasswordResetRequest : IRequestPasswordResetRequest
    {
        [PaginatorValidated]
        public string License { get; set; }
        [Required, EmailRegex(ErrorMessage = "Email address is invalid.")]
        public string Email { get; set; }

        public RequestPasswordResetRequest()
        {
            License = string.Empty;
            Email = string.Empty;
        }
    }
}
