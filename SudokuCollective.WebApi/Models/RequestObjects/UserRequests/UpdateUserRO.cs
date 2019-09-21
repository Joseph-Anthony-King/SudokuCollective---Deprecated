namespace SudokuCollective.WebApi.Models.RequestObjects.UserRequests {

    public class UpdateUserRO {

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
    }
}