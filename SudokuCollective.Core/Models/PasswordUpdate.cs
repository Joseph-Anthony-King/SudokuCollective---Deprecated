using System;
using Newtonsoft.Json;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Models
{
    public class PasswordUpdate : IPasswordUpdate
    {
        #region Properties
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AppId { get; set; }
        public string Token { get; set; }
        public DateTime DateCreated { get; set; }
        #endregion

        #region Constructors
        public PasswordUpdate()
        {
            Id = 0;
            UserId = 0;
            AppId = 0;
            Token = string.Empty;
            DateCreated = DateTime.MinValue;
        }

        public PasswordUpdate(int userId, int appId) : base()
        {
            UserId = userId;
            AppId = appId;
            Token = Guid.NewGuid().ToString();
            DateCreated = DateTime.UtcNow;
        }

        [JsonConstructor]
        public PasswordUpdate(int id, int userId, int appId, string token, DateTime dateCreated)
        {
            Id = id;
            UserId = userId;
            AppId = appId;
            Token = token;
            DateCreated = dateCreated;
        }
        #endregion
    }
}
