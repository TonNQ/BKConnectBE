using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.RoomManagement
{
    public class GroupRoomDto
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("total_member")]
        public long TotalMember { get; set; }
    }
}