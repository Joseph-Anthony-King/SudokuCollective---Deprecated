using SudokuCollective.Core.Interfaces.APIModels.RequestModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class RequestPasswordUpdateRequest : IRequestPasswordUpdateRequest
    {
        public string License { get; set; }
        public string Email { get; set; }

        public RequestPasswordUpdateRequest()
        {
            License = string.Empty;
            Email = string.Empty;
        }
    }
}
