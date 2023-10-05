using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.RefreshTokenManagement
{
    public class RefreshTokenDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("token")]
        public string Token { get; set; }
    }
}