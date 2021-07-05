using SudokuCollective.Core.Interfaces.APIModels.RequestModels;
using System.ComponentModel.DataAnnotations;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdatePasswordRequest : IPasswordResetRequest
    {
        [Required]
        public int UserId { get; set; }
        [Required]
        public string NewPassword { get; set; }

        public UpdatePasswordRequest() : base()
        {
            UserId = 0;
            NewPassword = string.Empty;
        }
    }
}
