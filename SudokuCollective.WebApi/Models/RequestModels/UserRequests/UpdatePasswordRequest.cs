namespace SudokuCollective.WebApi.Models.RequestModels.UserRequests {

    public class UpdatePasswordRequest : BaseRequest {

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }

        public UpdatePasswordRequest() : base() {

            OldPassword = string.Empty;
            NewPassword = string.Empty;
        }
    }
}
