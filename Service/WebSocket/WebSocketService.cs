using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.ChatManagement;
using BKConnectBE.Model.Dtos.MessageManagement;
using BKConnectBE.Model.Dtos.VideoCallManagement;
using BKConnectBE.Model.Dtos.WebSocketManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Service.Messages;
using BKConnectBE.Service.Notifications;
using BKConnectBE.Service.Rooms;
using BKConnectBE.Service.Users;
using BKConnectBE.Service.VideoCalls;

namespace BKConnectBE.Service.WebSocket
{
    public class WebSocketService : IWebSocketService
    {
        private readonly IUserService _userService;
        private readonly IMessageService _messageService;
        private readonly IRoomService _roomService;
        private readonly INotificationService _notificationService;
        private readonly IVideoCallService _videoCallService;
        private readonly IGenericRepository<Message> _genericRepositoryForMsg;
        private readonly IMapper _mapper;

        public WebSocketService(IUserService userService,
            IMessageService messageService,
            IRoomService roomService,
            INotificationService notificationService,
            IVideoCallService videoCallService,
            IGenericRepository<Message> genericRepository,
            IMapper mapper)
        {
            _userService = userService;
            _messageService = messageService;
            _roomService = roomService;
            _notificationService = notificationService;
            _videoCallService = videoCallService;
            _genericRepositoryForMsg = genericRepository;
            _mapper = mapper;
        }

        public void AddWebSocketConnection(WebSocketConnection connection)
        {
            StaticParams.WebsocketList.Add(connection);
        }

        public async Task CloseConnection(WebSocketConnection cnn)
        {
            if (IsInVideoCall(cnn.UserId))
            {
                var websocketOfVideoCall = new SendWebSocketData
                {
                    DataType = WebSocketDataType.IsVideoCall.ToString(),
                    VideoCall = new VideoCallData
                    {
                        RoomId = StaticParams.VideoCallList
                            .FirstOrDefault(cr => cr.UserIds.Contains(cnn.UserId))?.RoomId ?? 0,
                        VideoCallType = SystemMessageType.IsLeaveCall.ToString()
                    }
                };
                await LeaveVideoCall(websocketOfVideoCall, cnn.UserId);
            }

            var websocketData = new ReceiveWebSocketData
            {
                UserId = cnn.UserId,
                DataType = WebSocketDataType.IsOffline.ToString()
            };

            await _userService.UpdateLastOnlineAsync(cnn.UserId);
            await SendStatusMessage(websocketData);

            await cnn.WebSocket.CloseAsync(
                WebSocketCloseStatus.NormalClosure, "Disconnected", CancellationToken.None);
            StaticParams.WebsocketList.Remove(cnn);
        }

        public async Task SendStatusMessage(ReceiveWebSocketData websocketData)
        {
            var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(websocketData));

            var tasks = new List<Task>();

            foreach (WebSocketConnection webSocket in StaticParams.WebsocketList)
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
            var listOfWebSocket = StaticParams.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

            var receiveWebSocketData = new ReceiveWebSocketData
            {
                UserId = userId,
                DataType = websocketData.DataType,
                Message = newMsg
            };
            receiveWebSocketData.Message.TempId = websocketData.Message.TempId;

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
            var newMsg = await _messageService.AddMessageAsync(websocketData.Message, userId, receiverId);
            var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(websocketData.Message.RoomId);
            var listOfWebSocket = StaticParams.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

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
        public async Task SendSystemMessageForAddMember(SendWebSocketData websocketData, string userId, string receiverId, long newMsgId)
        {
            var newMsg = await _genericRepositoryForMsg.GetByIdAsync(newMsgId);
            var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(websocketData.Message.RoomId);
            var listOfWebSocket = StaticParams.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

            var receiveMsg = _mapper.Map<ReceiveMessageDto>(newMsg);
            receiveMsg.SendTime = receiveMsg.SendTime.AddHours(-7);

            var receiveWebSocketData = new ReceiveWebSocketData
            {
                UserId = userId,
                DataType = websocketData.DataType,
                Message = receiveMsg
            };

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var tasks = new List<Task>();

            foreach (WebSocketConnection webSocket in listOfWebSocket)
            {
                receiveWebSocketData.Message = await _messageService.ChangeSystemMessage(receiveWebSocketData.Message, webSocket.UserId, receiverId, SystemMessageType.IsInRoom.ToString());

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
            else if (websocketData.Notification.NotificationType == NotificationType.IsPostFile.ToString())
            {
                await SendPostFileInClassRoom(websocketData, userId);
            }
        }

        public async Task SendRoomNotification(SendWebSocketData websocketData, string userId, long roomId)
        {
            var notification = await _notificationService.AddRoomNotification(userId, websocketData.Notification.ReceiverId, websocketData.Notification.NotificationType, roomId);
            var webSocket = StaticParams.WebsocketList.FirstOrDefault(ws => ws.UserId == websocketData.Notification.ReceiverId);

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

        public async Task SendRoomInfo(SendWebSocketData websocketData, string userId)
        {
            var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(websocketData.RoomInfo.Id);
            var listOfWebSocket = StaticParams.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

            var receiveWebSocketData = new ReceiveWebSocketData
            {
                UserId = userId,
                DataType = websocketData.DataType,
                RoomInfo = websocketData.RoomInfo
            };

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var tasks = new List<Task>();
            foreach (WebSocketConnection webSocket in listOfWebSocket)
            {
                receiveWebSocketData.RoomInfo.LastMessage = await _messageService.ChangeContentSystemMessage(receiveWebSocketData.RoomInfo.LastMessageId ?? 0, webSocket.UserId);

                var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));

                tasks.Add(webSocket.WebSocket.SendAsync(
                    new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None));
            }

            await Task.WhenAll(tasks);
        }

        public async Task SendRoomInfoForNewMember(SendWebSocketData websocketData, string newUserId, string userId)
        {
            List<string> newUserIdList = newUserId.Split(", ").ToList();

            var listOfWebSocket = StaticParams.WebsocketList.Where(ws => newUserIdList.Contains(ws.UserId)).ToList();

            var receiveWebSocketData = new ReceiveWebSocketData
            {
                UserId = userId,
                DataType = websocketData.DataType,
                RoomInfo = websocketData.RoomInfo
            };

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var tasks = new List<Task>();
            foreach (WebSocketConnection webSocket in listOfWebSocket)
            {
                receiveWebSocketData.RoomInfo.LastMessage = await _messageService.ChangeContentSystemMessage(receiveWebSocketData.RoomInfo.LastMessageId ?? 0, webSocket.UserId);

                var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));

                tasks.Add(webSocket.WebSocket.SendAsync(
                    new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None));
            }

            await Task.WhenAll(tasks);
        }

        public async Task CallVideo(SendWebSocketData websocketData, string userId)
        {
            if (websocketData.VideoCall.VideoCallType == SystemMessageType.IsLeaveCall.ToString())
            {
                await LeaveVideoCall(websocketData, userId);
            }
            else if (websocketData.VideoCall.VideoCallType == SystemMessageType.IsJoinCall.ToString())
            {
                await JoinVideoCall(websocketData, userId);
            }
        }

        public async Task SendChangedRoomInfo(SendWebSocketData websocketData, string userId)
        {
            List<string> listOfUserId = new List<string>();
            if (websocketData.ChangedRoomInfo.NewMemberList != null)
            {
                List<string> changedUserIdList = new List<string>();
                foreach (var newUser in websocketData.ChangedRoomInfo.NewMemberList)
                {
                    changedUserIdList.Add(newUser.Id);
                }
                listOfUserId = await _roomService.GetListOfOldUserIdInRoomAsync(websocketData.ChangedRoomInfo.RoomId, changedUserIdList);

            }
            else if (websocketData.ChangedRoomInfo.LeftMemberId != null)
            {
                List<string> changedUserIdList = new List<string>
                {
                    websocketData.ChangedRoomInfo.LeftMemberId
                };
                listOfUserId = await _roomService.GetListOfOldUserIdInRoomAsync(websocketData.ChangedRoomInfo.RoomId, changedUserIdList);
            }
            else if (websocketData.ChangedRoomInfo.NewAvatar != null || websocketData.ChangedRoomInfo.NewName != null)
            {
                listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(websocketData.ChangedRoomInfo.RoomId);
            }

            var listOfWebSocket = StaticParams.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

            var receiveWebSocketData = new ReceiveWebSocketData
            {
                UserId = userId,
                DataType = websocketData.DataType,
                ChangedRoomInfo = websocketData.ChangedRoomInfo
            };

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var tasks = new List<Task>();
            var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));
            foreach (WebSocketConnection webSocket in listOfWebSocket)
            {
                tasks.Add(webSocket.WebSocket.SendAsync(
                    new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None));
            }

            await Task.WhenAll(tasks);
        }

        public async Task SendSignalForVideoCall(SendWebSocketData websocketData, string userId)
        {
            var webSocket = StaticParams.WebsocketList.FirstOrDefault(ws => ws.UserId == websocketData.SignalInfo.ToUser);

            if (webSocket is not null)
            {
                var receiveWebSocketData = new ReceiveWebSocketData
                {
                    UserId = userId,
                    DataType = WebSocketDataType.IsConnectSignal.ToString(),
                    SignalInfo = new SignalInfo
                    {
                        SignalType = websocketData.SignalInfo.SignalType,
                        SignalObject = websocketData.SignalInfo.SignalObject
                    }
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

        private async Task JoinVideoCall(SendWebSocketData websocketData, string userId)
        {
            if (IsInVideoCall(userId))
            {
                await SendErrorNotification(userId, MsgNo.ERROR_USER_ALREADY_IN_CALL);
                return;
            }

            var videoCall = StaticParams.VideoCallList.FirstOrDefault(cr => cr.RoomId == websocketData.VideoCall.RoomId);
            if (videoCall is not null)
            {
                videoCall.UserIds.Add(userId);
                await VideoCallSocket(websocketData, videoCall.RoomId, userId);
            }
            else
            {
                videoCall = new VideoCallWebsocket
                {
                    RoomId = websocketData.VideoCall.RoomId,
                    UserIds = new List<string> { userId }
                };
                StaticParams.VideoCallList.Add(videoCall);
                websocketData.VideoCall.VideoCallType = MessageType.IsStartCall.ToString();

                await VideoCallSocket(websocketData, videoCall.RoomId, userId);
            }
        }

        private async Task LeaveVideoCall(SendWebSocketData websocketData, string userId)
        {
            var videoCall = StaticParams.VideoCallList.FirstOrDefault(cr => cr.RoomId == websocketData.VideoCall.RoomId);
            if (videoCall is not null)
            {
                videoCall.UserIds.Remove(userId);

                if (videoCall.UserIds.Count == 0)
                {
                    StaticParams.VideoCallList.Remove(videoCall);
                    websocketData.VideoCall.VideoCallType = SystemMessageType.IsEndCall.ToString();
                }

                await VideoCallSocket(websocketData, videoCall.RoomId, userId);
            }
            else
            {
                await SendErrorNotification(userId, MsgNo.ERROR_USER_NOT_IN_CALL);
            }
        }

        private async Task VideoCallSocket(SendWebSocketData websocketData, long roomId, string userId)
        {
            var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(roomId);
            var listOfWebSocket = StaticParams.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var receiveWebSocketData = new ReceiveWebSocketData
            {
                UserId = userId,
                DataType = websocketData.DataType,
                VideoCall = new VideoCallData
                {
                    RoomId = roomId,
                    VideoCallType = websocketData.VideoCall.VideoCallType,
                    Participants = await _videoCallService.GetParticipantInfos(roomId)
                }
            };
            var tasks = new List<Task>();
            var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));

            foreach (WebSocketConnection webSocket in listOfWebSocket)
            {
                tasks.Add(webSocket.WebSocket.SendAsync(
                    new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None));
            }

            await Task.WhenAll(tasks);

            if (websocketData.VideoCall.VideoCallType == MessageType.IsStartCall.ToString())
            {
                // Gửi message thông báo cuộc gọi bắt đầu
                var webSocketMessage = new SendWebSocketData
                {
                    DataType = WebSocketDataType.IsMessage.ToString(),
                    Message = new SendMessageDto
                    {
                        RoomId = roomId,
                        Content = Constants.START_VIDEO_CALL,
                        TypeOfMessage = MessageType.IsStartCall.ToString()
                    }
                };
                await SendMessage(webSocketMessage, userId);

                // Gửi message thông báo người gọi đã join cuộc gọi
                webSocketMessage = new SendWebSocketData
                {
                    DataType = WebSocketDataType.IsMessage.ToString(),
                    Message = new SendMessageDto
                    {
                        RoomId = roomId,
                        Content = SystemMessageType.IsJoinCall.ToString(),
                        TypeOfMessage = MessageType.System.ToString()
                    }
                };
                await SendSystemMessage(webSocketMessage, userId, "", SystemMessageType.IsJoinCall.ToString());
            }
            else
            {
                var webSocketMessage = new SendWebSocketData
                {
                    DataType = WebSocketDataType.IsMessage.ToString(),
                    Message = new SendMessageDto
                    {
                        RoomId = roomId,
                        Content = websocketData.VideoCall.VideoCallType,
                        TypeOfMessage = MessageType.System.ToString()
                    }
                };
                await SendSystemMessage(webSocketMessage, userId, "", websocketData.VideoCall.VideoCallType);
            }
        }

        private async Task SendFriendRequest(SendWebSocketData websocketData, string userId)
        {
            var notification = await _notificationService.AddSendFriendRequestNotification(userId, websocketData.Notification.ReceiverId);
            var webSocket = StaticParams.WebsocketList.FirstOrDefault(ws => ws.UserId == websocketData.Notification.ReceiverId);

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
            var webSocket = StaticParams.WebsocketList.FirstOrDefault(ws => ws.UserId == websocketData.Notification.ReceiverId);

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

        private async Task SendPostFileInClassRoom(SendWebSocketData websocketData, string userId)
        {
            var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(websocketData.Notification.RoomId);

            var options = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                WriteIndented = true
            };

            var tasks = new List<Task>();

            foreach (string receivedId in listOfUserId)
            {
                if (receivedId != userId)
                {
                    var notification = await _notificationService.AddPostFileNotification(websocketData.Notification.FileId, receivedId);
                    var receiveWebSocketData = new ReceiveWebSocketData
                    {
                        UserId = userId,
                        DataType = websocketData.DataType,
                        Notification = notification
                    };
                    var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));

                    var webSocket = StaticParams.WebsocketList.FirstOrDefault(ws => ws.UserId == receivedId);
                    if (webSocket is not null)
                    {
                        tasks.Add(webSocket.WebSocket.SendAsync(
                            new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None));
                    }
                }
            }
            await Task.WhenAll(tasks);
        }

        private async Task SendErrorNotification(string userId, string errorMessage)
        {
            var webSocket = StaticParams.WebsocketList.FirstOrDefault(ws => ws.UserId == userId);

            if (webSocket is not null)
            {
                var receiveWebSocketData = new ReceiveWebSocketData
                {
                    UserId = userId,
                    DataType = WebSocketDataType.IsError.ToString(),
                    ErrorMessage = errorMessage
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

        private static bool IsInVideoCall(string userId)
        {
            return StaticParams.VideoCallList.Any(cr => cr.UserIds.Contains(userId));
        }
    }
}