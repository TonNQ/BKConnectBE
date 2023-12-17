using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.UserManagement
{
    public class ChangeRoomAvatarDto
    {
        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }
    }
}