using System;
using Newtonsoft.Json;

namespace Nop.Plugin.Widgets.BlogCategory.Models.Authentication
{
    public class TokenResponse
    {
        public TokenResponse(string accessToken, DateTime createdAtUtc, DateTime expiresAtUtc)
        {
            AccessToken = accessToken;
            CreatedAtUtc = createdAtUtc;
            ExpiresAtUtc = expiresAtUtc;
        }

        [JsonProperty("access_token", Required = Required.Always)]
        public string AccessToken { get; set; }

        [JsonProperty("token_type", Required = Required.Always)]
        public string TokenType { get; set; } = "Bearer";
        
        [JsonProperty("created_at_utc")]
        public DateTime CreatedAtUtc { get; set; }
        
        [JsonProperty("expires_at_utc")]
        public DateTime ExpiresAtUtc { get; set; }

        [JsonProperty("username")]
        public string Username { get; set; }

        [JsonProperty("customer_id")]
        public int CustomerId { get; set; }

        [JsonProperty("customer_guid")]
        public Guid CustomerGuid { get; set; }
    }
}
