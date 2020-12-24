using SudokuCollective.Core.Interfaces.APIModels.RequestModels;

namespace SudokuCollective.Data.Models.RequestModels
{
    public class UpdatePasswordRequest : IPasswordResetRequest
    {
        public int UserId { get; set; }
        public string NewPassword { get; set; }

        public UpdatePasswordRequest() : base()
        {
            UserId = 0;
            NewPassword = string.Empty;
        }
    }
}
