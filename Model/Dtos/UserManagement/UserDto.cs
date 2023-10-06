using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.UserManagement
{
    public class UserDto
    {
        [JsonPropertyName("user_id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("email")]
        public string Email { get; set; }

        [JsonPropertyName("gender")]
        public bool Gender { get; set; }

        [JsonPropertyName("birthday")]
        public DateTime BirthDay { get; set; }

        [JsonPropertyName("class")]
        public string Class { get; set; }

        [JsonPropertyName("role")]
        public string Role { get; set; }

        [JsonPropertyName("active")]
        public bool IsActive { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
    }
}
