namespace SudokuCollective.WebApi.Models.RequestModels.UserRequests {

    public class UpdateUserRO {

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
    }
}