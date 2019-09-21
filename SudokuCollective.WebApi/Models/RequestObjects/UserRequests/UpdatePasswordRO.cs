namespace SudokuCollective.WebApi.Models.RequestObjects.UserRequests {

    public class UpdatePasswordRO {

        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }
}
