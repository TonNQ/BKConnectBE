using System.Text.Json.Serialization;

namespace BKConnectBE.Model.Dtos.RoomManagement
{
    public class ChangedRoomDto
    {
        [JsonPropertyName("room_id")]
        public long RoomId { get; set; }

        [JsonPropertyName("total_member")]
        public int? TotalMember { get; set; }

        [JsonPropertyName("new_member_list")]
        public List<MemberOfRoomDto> NewMemberList { get; set; }

        [JsonPropertyName("left_member_id")]
        public string LeftMemberId { get; set; }

        [JsonPropertyName("new_avatar")]
        public string NewAvatar { get; set; }

        [JsonPropertyName("new_name")]
        public string NewName { get; set; }
    }
}