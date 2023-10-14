using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.UserManagement
{
    public class UserSearchDto
    {
        [JsonPropertyName("user_id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }
    }
}
