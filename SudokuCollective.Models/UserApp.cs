using Newtonsoft.Json;

namespace SudokuCollective.Models {

    public class UserApp {

        public int UserId { get; set; }
        public User User { get; set; }

        public int AppId { get; set; }
        public App App { get; set; }

        public UserApp() {

            UserId = 0;
            User = null;
            AppId = 0;
            App = null;
        }

        [JsonConstructor]
        public UserApp(int userId, int appId) : this() {

            UserId = userId;
            AppId = appId;
        }
    }
}
