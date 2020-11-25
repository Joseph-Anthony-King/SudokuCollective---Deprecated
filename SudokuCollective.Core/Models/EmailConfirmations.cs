using System;
using Newtonsoft.Json;
using SudokuCollective.Core.Interfaces.Models;

namespace SudokuCollective.Core.Models
{
    public class EmailConfirmation : IEmailConfirmation
    {
        #region Properties
        public int Id { get; set; }
        public int UserId { get; set; }
        public int AppId { get; set; }
        public string Token { get; set; }
        #endregion

        #region Constructors
        public EmailConfirmation()
        {
            Id = 0;
            UserId = 0;
            AppId = 0;
            Token = string.Empty;
        }

        public EmailConfirmation(int userId, int appId) : base()
        {
            UserId = userId;
            AppId = appId;
            Token = Guid.NewGuid().ToString();
        }

        [JsonConstructor]
        public EmailConfirmation(
            int id, 
            int userId,
            int appId,
            string code)
        {
            Id = id;
            UserId = userId;
            AppId = appId;
            Token = code;
        }
        #endregion
    }
}
