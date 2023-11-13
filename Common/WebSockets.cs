using BKConnectBE.Model.Dtos.WebSocketManagement;

namespace BKConnectBE.Common
{
    public class WebSockets
    {
        public static List<WebSocketConnection> WebsocketList { get; set; } = new();
    }
}