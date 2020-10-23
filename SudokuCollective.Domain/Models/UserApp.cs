using Newtonsoft.Json;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Domain.Models
{
    public class UserApp : IUserApp
    {
        #region Properties
        public int Id { get; set; }
        public int UserId { get; set; }
        public IUser User { get; set; }
        public int AppId { get; set; }
        public IApp App { get; set; }
        #endregion

        #region Constructors
        public UserApp()
        {
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
            int appId)
        {
            Id = id;
            UserId = userId;
            AppId = appId;
        }
        #endregion
    }
}
