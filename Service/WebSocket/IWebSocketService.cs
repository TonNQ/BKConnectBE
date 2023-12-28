using BKConnectBE.Model.Dtos.ChatManagement;
using BKConnectBE.Model.Dtos.WebSocketManagement;

namespace BKConnectBE.Service.WebSocket
{
    public interface IWebSocketService
    {
        void AddWebSocketConnection(WebSocketConnection connection);
        Task CloseConnection(WebSocketConnection cnn);
        Task SendStatusMessage(ReceiveWebSocketData websocketData);
        Task SendMessage(SendWebSocketData websocketData, string userId);
        Task SendNotification(SendWebSocketData websocketData, string userId);
        Task SendSystemMessage(SendWebSocketData websocketData, string userId, string receiverId, string type);
        Task SendSystemMessageForAddMember(SendWebSocketData websocketData, string userId, string receiverId, long newMsgId);
        Task SendRoomInfo(SendWebSocketData websocketData, string userId);
        Task SendRoomInfoForNewMember(SendWebSocketData websocketData, string newUserId, string userId);
        Task SendRoomNotification(SendWebSocketData websocketData, string userId, long roomId);
        Task SendChangedRoomInfo(SendWebSocketData websocketData, string userId);
        Task CallVideo(SendWebSocketData websocketData, string userId);
        Task SendSignalForVideoCall(SendWebSocketData websocketData, string userId);
        Task SendErrorNotification(string userId, string errorMessage);
    }
}