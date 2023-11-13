using BKConnectBE.Model.Dtos.ChatManagement;
using BKConnectBE.Model.Dtos.WebSocketManagement;

namespace BKConnectBE.Service.WebSocket
{
    public interface IWebSocketService
    {
        void AddWebSocketConnection(WebSocketConnection connection);
        Task CloseConnection(WebSocketConnection cnn);
        Task SendStatusMessage(ReceiveWebSocketData websocketData);
        Task SendTextMessage(SendWebSocketData websocketData, string userId);
        Task SendFriendRequest(SendWebSocketData websocketData, string userId);
    }
}