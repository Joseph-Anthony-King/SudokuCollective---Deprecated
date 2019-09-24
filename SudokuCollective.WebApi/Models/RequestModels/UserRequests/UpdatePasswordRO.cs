namespace SudokuCollective.WebApi.Models.RequestModels.UserRequests {

    public class UpdatePasswordRO : BaseRequestRO {

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
