using System.ComponentModel.DataAnnotations;

namespace SudokuCollective.Api.Models
{
    public class PasswordUpdate
    {
        public bool Success { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please enter your new password")]
        [Compare("ConfirmNewPassword", ErrorMessage = "Must match the value of confirm new password")]
        public string NewPassword { get; set; }
        [Required(AllowEmptyStrings = false, ErrorMessage = "Please confirm your new password")]
        public string ConfirmNewPassword { get; set; }
        public string AppTitle { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
    }
}
