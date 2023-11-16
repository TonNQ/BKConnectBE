using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.RoomManagement
{
    public class AddMemberDto
    {
        [JsonPropertyName("user_id")]
        public string UserId { get; set; }

        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }
    }
}