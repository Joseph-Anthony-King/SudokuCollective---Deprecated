using Newtonsoft.Json;
using SudokuCollective.Core.Interfaces.APIModels.TokenModels;

namespace SudokuCollective.Data.Models.TokenModels
{
    [JsonObject("TokenManagement")]
    public class TokenManagement : ITokenManagement
    {
        [JsonProperty("Secret")]
        public string Secret { get; set; }
        [JsonProperty("Issuer")]
        public string Issuer { get; set; }
        [JsonProperty("Audience")]
        public string Audience { get; set; }
        [JsonProperty("AccessExpiration")]
        public int AccessExpiration { get; set; }
        [JsonProperty("RefreshExpiration")]
        public int RefreshExpiration { get; set; }
    }
}
