using BKConnectBE.Model.Dtos.ChatManagement;
using BKConnectBE.Model.Dtos.WebSocketManagement;

namespace BKConnectBE.Service.WebSocket
{
    public interface IWebSocketService
    {
        void AddWebSocketConnection(WebSocketConnection connection);
        Task CloseConnection(WebSocketConnection cnn);
        Task SendStatusMessage(ReceiveWebSocketData websocketData, string wsId);
        Task SendMessage(SendWebSocketData websocketData, WebSocketConnection cnn);
        Task SendNotification(SendWebSocketData websocketData, WebSocketConnection cnn);
        Task SendSystemMessage(SendWebSocketData websocketData, WebSocketConnection cnn, string receiverId, string type);
        Task SendSystemMessageForAddMember(SendWebSocketData websocketData, WebSocketConnection cnn, string receiverId, long newMsgId);
        Task SendRoomInfo(SendWebSocketData websocketData, WebSocketConnection cnn);
        Task SendRoomInfoForNewMember(SendWebSocketData websocketData, string newUserId, WebSocketConnection cnn);
        Task SendRoomNotification(SendWebSocketData websocketData, WebSocketConnection cnn, long roomId);
        Task SendChangedRoomInfo(SendWebSocketData websocketData, WebSocketConnection cnn);
        Task CallVideo(SendWebSocketData websocketData, WebSocketConnection cnn);
        Task SendSignalForVideoCall(SendWebSocketData websocketData, WebSocketConnection cnn);
        Task SendErrorNotification(WebSocketConnection cnn, string errorMessage);
    }
}