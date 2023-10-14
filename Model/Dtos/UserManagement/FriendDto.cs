using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.UserManagement
{
    public class FriendDto
    {
        [JsonPropertyName("user_id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("class_name")]
        public string ClassName { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
    }
}