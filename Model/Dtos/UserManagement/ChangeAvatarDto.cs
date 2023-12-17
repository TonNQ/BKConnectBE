using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.UserManagement
{
    public class ChangeAvatarDto
    {
        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
    }
}