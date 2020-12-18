namespace SudokuCollective.Api.Models
{
    public class ConfirmEmail
    {
        public bool Success { get; set; }
        public string UserName { get; set; }
        public string AppTitle { get; set; }
        public string Url { get; set; }
        public bool IsUpdate { get; set; }
        public bool NewEmailAddressConfirmed { get; set; }
    }
}
