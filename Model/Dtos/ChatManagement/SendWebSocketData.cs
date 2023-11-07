using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.FriendRequestManagement;
using BKConnectBE.Model.Dtos.MessageManagement;

namespace BKConnectBE.Model.Dtos.ChatManagement
{
    public class SendWebSocketData
    {
        [JsonPropertyName("data_type")]
        [EnumDataType(typeof(WebSocketDataType))]
        public string DataType { get; set; }

        [JsonPropertyName("send_message")]
        public SendMessageDto SendMessage { get; set; } = null;

        [JsonPropertyName("friend_request")]
        public FriendRequestDto FriendRequest { get; set; } = null;
    }
}