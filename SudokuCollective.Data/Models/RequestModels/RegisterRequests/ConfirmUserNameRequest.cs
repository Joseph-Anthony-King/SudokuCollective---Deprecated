using SudokuCollective.Core.Interfaces.APIModels.RequestModels.RegisterRequests;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class ConfirmUserNameRequest : IConfirmUserNameRequest
    {
        public string Email { get; set; }
        public string License { get; set; }

        public ConfirmUserNameRequest()
        {
            Email = string.Empty;
            License = string.Empty;
        }
    }
}
