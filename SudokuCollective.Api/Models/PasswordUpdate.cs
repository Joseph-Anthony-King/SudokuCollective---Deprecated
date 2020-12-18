namespace SudokuCollective.Api.Models
{
    public class PasswordUpdate
    {
        public bool Success { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string OldPassword { get; set; }
        public string ConfirmOldPassword { get; set; }
        public string NewPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public string AppTitle { get; set; }
        public string Url { get; set; }
        public string Message { get; set; }
    }
}
