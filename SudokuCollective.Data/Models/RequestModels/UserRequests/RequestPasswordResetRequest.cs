using SudokuCollective.Core.Interfaces.APIModels.RequestModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class RequestPasswordResetRequest : IRequestPasswordResetRequest
    {
        public string License { get; set; }
        public string Email { get; set; }

        public RequestPasswordResetRequest()
        {
            License = string.Empty;
            Email = string.Empty;
        }
    }
}
