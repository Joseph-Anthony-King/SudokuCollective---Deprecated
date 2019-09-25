using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace SudokuCollective.WebApi.Models {

    public class TokenRequest {

        [Required]
        [JsonProperty("UserName")]
        public string UserName { get; set; }

        [Required]
        [JsonProperty("Password")]
        public string Password { get; set; }
    }
}
