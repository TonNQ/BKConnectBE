using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.ChatManagement;
using BKConnectBE.Model.Dtos.WebSocketManagement;
using BKConnectBE.Service.FriendRequests;
using BKConnectBE.Service.Messages;
using BKConnectBE.Service.Rooms;
using BKConnectBE.Service.Users;

namespace BKConnectBE.Service.WebSocket
{
    public class WebSocketService : IWebSocketService
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IRoomService _roomService;
        private readonly IFriendRequestService _friendRequestService;

        public WebSocketService(IUserService userService, 
            IMessageService messageService, 
            IRoomService roomService, 
            IFriendRequestService friendRequestService)
        {
            _userService = userService;
            _messageService = messageService;
            _roomService = roomService;
            _friendRequestService = friendRequestService;
        }

        public void AddWebSocketConnection(WebSocketConnection connection)
        {
            WebSockets.WebsocketList.Add(connection);
        }

        public async Task CloseConnection(WebSocketConnection cnn)
        {
            var websocketData = new ReceiveWebSocketData
            {
                UserId = cnn.UserId,
                DataType = WebSocketDataType.IsOffline.ToString()
            };

            await _userService.UpdateLastOnlineAsync(cnn.UserId);
            await SendStatusMessage(websocketData);

            await cnn.WebSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure, "Disconnected", CancellationToken.None);
            WebSockets.WebsocketList.Remove(cnn);
        }

        public async Task SendStatusMessage(ReceiveWebSocketData websocketData)
        {
            var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(websocketData));

            var tasks = new List<Task>();

            foreach (WebSocketConnection webSocket in WebSockets.WebsocketList)
            {
                tasks.Add(webSocket.WebSocket.SendAsync(
                    new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None));
            }

            await Task.WhenAll(tasks);
        }

        public async Task SendTextMessage(SendWebSocketData websocketData, string userId)
        {
            var newMsg = await _messageService.AddMessageAsync(websocketData.Message, userId);
            var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(websocketData.Message.RoomId);
            var listOfWebSocket = WebSockets.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

            var receiveWebSocketData = new ReceiveWebSocketData
            {
                UserId = userId,
                DataType = websocketData.DataType,
                Message = newMsg
            };
            string rootMessageSenderId = await _messageService.GetRootMessageSenderId(websocketData.Message.RootMessageId);

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var tasks = new List<Task>();

            foreach (WebSocketConnection webSocket in listOfWebSocket)
            {
                receiveWebSocketData.Message = await _messageService.RenameUser(receiveWebSocketData.Message, webSocket.UserId, rootMessageSenderId);
                var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));

                tasks.Add(webSocket.WebSocket.SendAsync(
                    new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None));
            }

            await Task.WhenAll(tasks);
        }

        public async Task SendFriendRequest(SendWebSocketData websocketData, string userId)
        {
            var friendRequest = await _friendRequestService.CreateFriendRequest(userId, websocketData.FriendRequest.ReceiverId);
            var webSocket = WebSockets.WebsocketList.FirstOrDefault(ws => ws.UserId == websocketData.FriendRequest.ReceiverId);

            if (webSocket is not null)
            {
                var receiveWebSocketData = new ReceiveWebSocketData
                {
                    UserId = userId,
                    DataType = websocketData.DataType,
                    FriendRequest = friendRequest
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                };
                var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));
                var tasks = new List<Task>();

                await webSocket.WebSocket.SendAsync(
                    new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
        }
    }
}