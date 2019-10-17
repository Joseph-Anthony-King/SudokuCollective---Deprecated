using Newtonsoft.Json;
using SudokuCollective.Domain.Interfaces;

namespace SudokuCollective.Domain.Models {

    public class UserApp : IUserApp {

        public int Id { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public int AppId { get; set; }
        public App App { get; set; }

        public UserApp() {

            Id = 0;
            UserId = 0;
            User = null;
            AppId = 0;
            App = null;
        }

        [JsonConstructor]
        public UserApp(
            int id, 
            int userId, 
            int appId) : this() {

            Id = id;
            UserId = userId;
            AppId = appId;
        }
    }
}
