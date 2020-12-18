using SudokuCollective.Core.Interfaces.APIModels.ResultModels.UserResults;

namespace SudokuCollective.Data.Models.ResultModels
{
    public class ConfirmEmailResult : IConfirmEmailResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
        public string AppTitle { get; set; }
        public string Url { get; set; }
        public bool? IsUpdate { get; set; }
        public bool? NewEmailAddressConfirmed { get; set; }
        public bool? ConfirmationEmailSuccessfullySent { get; set; }

        public ConfirmEmailResult() : base()
        {
            UserName = string.Empty;
            AppTitle = string.Empty;
            Url = string.Empty;
            IsUpdate = null;
            NewEmailAddressConfirmed = null;
            ConfirmationEmailSuccessfullySent = null;
        }
    }
}
