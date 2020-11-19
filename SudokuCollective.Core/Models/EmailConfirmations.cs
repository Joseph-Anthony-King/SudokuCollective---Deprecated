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
        public string Code { get; set; }
        #endregion

        #region Constructors
        public EmailConfirmation()
        {
            Id = 0;
            UserId = 0;
            Code = string.Empty;
        }

        public EmailConfirmation(int userId) : base()
        {
            UserId = userId;
            Code = Guid.NewGuid().ToString();
        }

        [JsonConstructor]
        public EmailConfirmation(int id, int userId, string code)
        {
            Id = id;
            UserId = userId;
            Code = code;
        }
        #endregion
    }
}
