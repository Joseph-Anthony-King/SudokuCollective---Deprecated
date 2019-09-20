using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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
