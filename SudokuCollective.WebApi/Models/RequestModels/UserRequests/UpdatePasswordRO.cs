namespace SudokuCollective.WebApi.Models.RequestModels.UserRequests {

    public class UpdatePasswordRO {

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
