using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.RoomManagement
{
    public class AddGroupRoomDto
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("avatar")]
        public string Avatar { get; set; }

        [JsonPropertyName("room_type")]
        public string RoomType { get; set; }

        [JsonPropertyName("school_year_id")]
        public long? SchoolYearId { get; set; }

        [JsonPropertyName("user_ids")]
        public List<string> UserIds { get; set; }
    }
}