using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.RoomManagement
{
    public class ReadMessageOfRoomDto
    {
        [JsonPropertyName("message_id")]
        public long Id { get; set; }
    }
}