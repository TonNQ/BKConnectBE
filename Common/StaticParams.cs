using BKConnectBE.Model.Dtos.VideoCallManagement;
using BKConnectBE.Model.Dtos.WebSocketManagement;

namespace BKConnectBE.Common
{
    public class StaticParams
    {
        public static List<WebSocketConnection> WebsocketList { get; set; } = new();

        public static List<VideoCallWebsocket> VideoCallList { get; set; } = new();
    }
}