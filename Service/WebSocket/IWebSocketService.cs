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
    }
}