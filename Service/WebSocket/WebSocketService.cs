using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.ChatManagement;
using BKConnectBE.Model.Dtos.WebSocketManagement;
using BKConnectBE.Service.Messages;
using BKConnectBE.Service.Notifications;
using BKConnectBE.Service.Rooms;
using BKConnectBE.Service.Users;

namespace BKConnectBE.Service.WebSocket
{
    public class WebSocketService : IWebSocketService
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IRoomService _roomService;
        private readonly INotificationService _notificationService;

        public WebSocketService(IUserService userService,
            IMessageService messageService,
            IRoomService roomService,
            INotificationService notificationService)
        {
            _userService = userService;
            _messageService = messageService;
            _roomService = roomService;
            _notificationService = notificationService;
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

        public async Task SendMessage(SendWebSocketData websocketData, string userId)
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
                receiveWebSocketData.Message = await _messageService.ChangeMessage(receiveWebSocketData.Message, webSocket.UserId, rootMessageSenderId);

                var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));

                tasks.Add(webSocket.WebSocket.SendAsync(
                    new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None));
            }

            await Task.WhenAll(tasks);
        }

        public async Task SendSystemMessage(SendWebSocketData websocketData, string userId, string receiverId, string type)
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

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var tasks = new List<Task>();

            foreach (WebSocketConnection webSocket in listOfWebSocket)
            {
                receiveWebSocketData.Message = await _messageService.ChangeSystemMessage(receiveWebSocketData.Message, webSocket.UserId, receiverId, type);

                var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));

                tasks.Add(webSocket.WebSocket.SendAsync(
                    new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None));
            }

            await Task.WhenAll(tasks);
        }

        public async Task SendNotification(SendWebSocketData websocketData, string userId)
        {
            if (websocketData.Notification.NotificationType == NotificationType.IsSendFriendRequest.ToString())
            {
                await SendFriendRequest(websocketData, userId);
            }
            else if (websocketData.Notification.NotificationType == NotificationType.IsAcceptFriendRequest.ToString())
            {
                await SendFriendRequestAcception(websocketData, userId);
            }
        }

        private async Task SendFriendRequest(SendWebSocketData websocketData, string userId)
        {
            var notification = await _notificationService.AddSendFriendRequestNotification(userId, websocketData.Notification.ReceiverId);
            var webSocket = WebSockets.WebsocketList.FirstOrDefault(ws => ws.UserId == websocketData.Notification.ReceiverId);

            if (webSocket is not null)
            {
                var receiveWebSocketData = new ReceiveWebSocketData
                {
                    UserId = userId,
                    DataType = websocketData.DataType,
                    Notification = notification
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

        private async Task SendFriendRequestAcception(SendWebSocketData websocketData, string userId)
        {
            var notification = await _notificationService.AddAcceptedFriendRequestNotification(userId, websocketData.Notification.ReceiverId);
            var webSocket = WebSockets.WebsocketList.FirstOrDefault(ws => ws.UserId == websocketData.Notification.ReceiverId);

            if (webSocket is not null)
            {
                var receiveWebSocketData = new ReceiveWebSocketData
                {
                    UserId = userId,
                    DataType = websocketData.DataType,
                    Notification = notification
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

        public async Task SendRoomNotification(SendWebSocketData websocketData, string userId, long roomId)
        {
            var notification = await _notificationService.AddRoomNotification(userId, websocketData.Notification.ReceiverId, websocketData.Notification.NotificationType, roomId);
            var webSocket = WebSockets.WebsocketList.FirstOrDefault(ws => ws.UserId == websocketData.Notification.ReceiverId);

            if (webSocket is not null)
            {
                var receiveWebSocketData = new ReceiveWebSocketData
                {
                    UserId = userId,
                    DataType = websocketData.DataType,
                    Notification = notification
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