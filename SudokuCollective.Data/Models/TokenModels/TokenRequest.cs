using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using SudokuCollective.Core.Interfaces.APIModels.TokenModels;

namespace SudokuCollective.Data.Models.TokenModels
{
    public class TokenRequest : ITokenRequest
    {
        [Required]
        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [Required]
        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
