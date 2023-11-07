using System.Net.WebSockets;

namespace BKConnectBE.Model.Dtos.WebSocketManagement
{
    public class WebSocketConnection
    {
        public string UserId { get; set; }

        public WebSocket WebSocket { get; set; }
    }
}