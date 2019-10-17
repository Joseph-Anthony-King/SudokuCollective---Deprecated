namespace SudokuCollective.WebApi.Models.RequestModels.RegisterRequests {
    
    public class RegisterRequest : BaseRequest {

        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public RegisterRequest() : base() {

            UserName = string.Empty;
            FirstName = string.Empty;
            LastName = string.Empty;
            NickName = string.Empty;
            Email = string.Empty;
            Password = string.Empty;
        }
    }
}
