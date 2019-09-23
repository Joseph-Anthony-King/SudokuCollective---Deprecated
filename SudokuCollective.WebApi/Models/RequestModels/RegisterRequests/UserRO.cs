namespace SudokuCollective.WebApi.Models.RequestModels.RegisterRequests {
    
    public class RegisterRO {

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
