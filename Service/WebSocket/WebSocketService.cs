using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using AutoMapper;
using AutoMapper.Internal;
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
        private readonly IGenericRepository<Room> _genericRepositoryForRoom;
        private readonly IMapper _mapper;

        public WebSocketService(IUserService userService,
            IMessageService messageService,
            IRoomService roomService,
            INotificationService notificationService,
            IVideoCallService videoCallService,
            IGenericRepository<Message> genericRepositoryForMsg,
            IGenericRepository<Room> genericRepositoryForRoom,
            IMapper mapper)
        {
            _userService = userService;
            _messageService = messageService;
            _roomService = roomService;
            _notificationService = notificationService;
            _videoCallService = videoCallService;
            _genericRepositoryForMsg = genericRepositoryForMsg;
            _genericRepositoryForRoom = genericRepositoryForRoom;
            _mapper = mapper;
        }

        public void AddWebSocketConnection(WebSocketConnection connection)
        {
            if (!StaticParams.WebsocketList.TryAdd(connection))
            {
                StaticParams.WebsocketList.Remove(connection);
                StaticParams.WebsocketList.TryAdd(connection);
            }
        }

        public async Task CloseConnection(WebSocketConnection cnn)
        {
            try
            {
                StaticParams.WebsocketList.Remove(cnn);
                if (IsInVideoCall(cnn.UserId))
                {
                    var websocketOfVideoCall = new SendWebSocketData
                    {
                        DataType = WebSocketDataType.IsVideoCall.ToString(),
                        VideoCall = new VideoCallData
                        {
                            RoomId = StaticParams.VideoCallList
                                .FirstOrDefault(cr => cr.Participants.ContainsKey(cnn.UserId))?.RoomId ?? 0,
                            VideoCallType = SystemMessageType.IsLeaveCall.ToString()
                        }
                    };
                    await LeaveVideoCall(websocketOfVideoCall, cnn);
                }

                var websocketData = new ReceiveWebSocketData
                {
                    UserId = cnn.UserId,
                    DataType = WebSocketDataType.IsOffline.ToString()
                };

                await _userService.UpdateLastOnlineAsync(cnn.UserId);
                await SendStatusMessage(websocketData, cnn.Id);

                await cnn.WebSocket.CloseAsync(
                    WebSocketCloseStatus.NormalClosure, "Disconnected", CancellationToken.None);
            }
            catch (Exception)
            {
                StaticParams.WebsocketList.Remove(cnn);
            }
        }

        public async Task SendStatusMessage(ReceiveWebSocketData websocketData, string wsId)
        {
            try
            {
                var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(websocketData));

                var tasks = new List<Task>();

                foreach (WebSocketConnection webSocket in StaticParams.WebsocketList)
                {
                    if (webSocket.WebSocket.State == WebSocketState.Closed)
                    {
                        StaticParams.WebsocketList.Remove(webSocket);
                        continue;
                    }
                    tasks.Add(webSocket.WebSocket.SendAsync(
                        new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None));
                }

                await Task.WhenAll(tasks);
            }
            catch (Exception)
            {
                var cnn = StaticParams.WebsocketList.FirstOrDefault(ws => ws.Id == wsId);
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }

        public async Task SendMessage(SendWebSocketData websocketData, WebSocketConnection cnn)
        {
            try
            {
                var newMsg = await _messageService.AddMessageAsync(websocketData.Message, cnn.UserId);
                var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(websocketData.Message.RoomId);
                var listOfWebSocket = StaticParams.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

                if (listOfWebSocket.Count > 0)
                {
                    var receiveWebSocketData = new ReceiveWebSocketData
                    {
                        UserId = cnn.UserId,
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
                        if (webSocket.WebSocket.State == WebSocketState.Closed)
                        {
                            StaticParams.WebsocketList.Remove(webSocket);
                            continue;
                        }

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
            }
            catch (Exception)
            {
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }

        public async Task SendSystemMessage(SendWebSocketData websocketData, WebSocketConnection cnn, string receiverId, string type)
        {
            try
            {
                var newMsg = await _messageService.AddMessageAsync(websocketData.Message, cnn.UserId, receiverId);
                var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(websocketData.Message.RoomId);
                var listOfWebSocket = StaticParams.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

                if (listOfWebSocket.Count > 0)
                {
                    var receiveWebSocketData = new ReceiveWebSocketData
                    {
                        UserId = cnn.UserId,
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
                        if (webSocket.WebSocket.State == WebSocketState.Closed)
                        {
                            StaticParams.WebsocketList.Remove(webSocket);
                            continue;
                        }

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
            }
            catch (Exception)
            {
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }
        public async Task SendSystemMessageForAddMember(SendWebSocketData websocketData, WebSocketConnection cnn, string receiverId, long newMsgId)
        {
            try
            {
                var newMsg = await _genericRepositoryForMsg.GetByIdAsync(newMsgId);
                var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(websocketData.Message.RoomId);
                var listOfWebSocket = StaticParams.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

                if (listOfWebSocket.Count > 0)
                {
                    var receiveMsg = _mapper.Map<ReceiveMessageDto>(newMsg);
                    receiveMsg.SendTime = receiveMsg.SendTime.AddHours(-7);

                    var receiveWebSocketData = new ReceiveWebSocketData
                    {
                        UserId = cnn.UserId,
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
                        if (webSocket.WebSocket.State == WebSocketState.Closed)
                        {
                            StaticParams.WebsocketList.Remove(webSocket);
                            continue;
                        }

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
            }
            catch (Exception)
            {
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }
        public async Task SendNotification(SendWebSocketData websocketData, WebSocketConnection cnn)
        {
            try
            {
                if (websocketData.Notification.NotificationType == NotificationType.IsSendFriendRequest.ToString())
                {
                    await SendFriendRequest(websocketData, cnn.UserId);
                }
                else if (websocketData.Notification.NotificationType == NotificationType.IsAcceptFriendRequest.ToString())
                {
                    await SendFriendRequestAcception(websocketData, cnn.UserId);
                    await SendFriendRequestAcception(cnn.UserId);
                    await SendFriendRequestAcception(websocketData.Notification.ReceiverId);
                }
                else if (websocketData.Notification.NotificationType == NotificationType.IsPostFile.ToString())
                {
                    await SendPostFileInClassRoom(websocketData, cnn.UserId);
                }
            }
            catch (Exception)
            {
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }

        public async Task SendRoomNotification(SendWebSocketData websocketData, WebSocketConnection cnn, long roomId)
        {
            try
            {
                var notification = await _notificationService.AddRoomNotification(cnn.UserId, websocketData.Notification.ReceiverId, websocketData.Notification.NotificationType, roomId);
                var listOfWebSocket = StaticParams.WebsocketList.Where(ws => ws.UserId == websocketData.Notification.ReceiverId).ToList();

                if (listOfWebSocket.Count > 0)
                {
                    var receiveWebSocketData = new ReceiveWebSocketData
                    {
                        UserId = cnn.UserId,
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

                    foreach (WebSocketConnection webSocket in listOfWebSocket)
                    {
                        if (webSocket.WebSocket.State == WebSocketState.Closed)
                        {
                            StaticParams.WebsocketList.Remove(webSocket);
                            continue;
                        }

                        tasks.Add(webSocket.WebSocket.SendAsync(
                            new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None));
                    }
                }
            }
            catch (Exception)
            {
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }

        public async Task SendRoomInfo(SendWebSocketData websocketData, WebSocketConnection cnn)
        {
            try
            {
                var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(websocketData.RoomInfo.Id);
                var listOfWebSocket = StaticParams.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

                if (listOfWebSocket.Count > 0)
                {
                    var receiveWebSocketData = new ReceiveWebSocketData
                    {
                        UserId = cnn.UserId,
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
                        if (webSocket.WebSocket.State == WebSocketState.Closed)
                        {
                            StaticParams.WebsocketList.Remove(webSocket);
                            continue;
                        }

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
            }
            catch (Exception)
            {
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }

        public async Task SendRoomInfoForNewMember(SendWebSocketData websocketData, string newUserId, WebSocketConnection cnn)
        {
            try
            {
                List<string> newUserIdList = newUserId.Split(", ").ToList();

                var listOfWebSocket = StaticParams.WebsocketList.Where(ws => newUserIdList.Contains(ws.UserId)).ToList();

                if (listOfWebSocket.Count > 0)
                {
                    var receiveWebSocketData = new ReceiveWebSocketData
                    {
                        UserId = cnn.UserId,
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
                        if (webSocket.WebSocket.State == WebSocketState.Closed)
                        {
                            StaticParams.WebsocketList.Remove(webSocket);
                            continue;
                        }

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
            }
            catch (Exception)
            {
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }

        public async Task CallVideo(SendWebSocketData websocketData, WebSocketConnection cnn)
        {
            try
            {
                var room = await _genericRepositoryForRoom.GetByIdAsync(websocketData.VideoCall.RoomId);
                if (room is null)
                {
                    await SendErrorNotification(cnn, MsgNo.ERROR_ROOM_NOT_FOUND);
                    return;
                }
                if (websocketData.VideoCall.VideoCallType == SystemMessageType.IsLeaveCall.ToString())
                {
                    await LeaveVideoCall(websocketData, cnn);
                }
                else if (websocketData.VideoCall.VideoCallType == SystemMessageType.IsJoinCall.ToString())
                {
                    await JoinVideoCall(websocketData, cnn);
                }
            }
            catch (Exception)
            {
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }

        public async Task SendChangedRoomInfo(SendWebSocketData websocketData, WebSocketConnection cnn)
        {
            try
            {
                List<string> listOfUserId = new();
                if (websocketData.ChangedRoomInfo.NewMemberList != null)
                {
                    List<string> changedUserIdList = new();
                    foreach (var newUser in websocketData.ChangedRoomInfo.NewMemberList)
                    {
                        changedUserIdList.Add(newUser.Id);
                    }
                    listOfUserId = await _roomService.GetListOfOldUserIdInRoomAsync(websocketData.ChangedRoomInfo.RoomId, changedUserIdList);

                }
                else if (websocketData.ChangedRoomInfo.LeftMemberId != null)
                {
                    List<string> changedUserIdList = new()
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

                if (listOfWebSocket.Count > 0)
                {
                    var receiveWebSocketData = new ReceiveWebSocketData
                    {
                        UserId = cnn.UserId,
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
                        if (webSocket.WebSocket.State == WebSocketState.Closed)
                        {
                            StaticParams.WebsocketList.Remove(webSocket);
                            continue;
                        }

                        tasks.Add(webSocket.WebSocket.SendAsync(
                            new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None));
                    }

                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception)
            {
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }

        public async Task SendSignalForVideoCall(SendWebSocketData websocketData, WebSocketConnection cnn)
        {
            try
            {
                var listOfWebSocket = StaticParams.WebsocketList.Where(ws => ws.UserId == websocketData.SignalInfo.ToUser).ToList();

                if (listOfWebSocket.Count > 0)
                {
                    var receiveWebSocketData = new ReceiveWebSocketData
                    {
                        UserId = cnn.UserId,
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

                    foreach (WebSocketConnection webSocket in listOfWebSocket)
                    {
                        if (webSocket.WebSocket.State == WebSocketState.Closed)
                        {
                            StaticParams.WebsocketList.Remove(webSocket);
                            continue;
                        }

                        tasks.Add(webSocket.WebSocket.SendAsync(
                            new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                            WebSocketMessageType.Text,
                            true,
                            CancellationToken.None));
                    }
                    await Task.WhenAll(tasks);
                }
            }
            catch (Exception)
            {
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }

        public async Task SendErrorNotification(WebSocketConnection cnn, string errorMessage)
        {
            try
            {
                var webSocket = StaticParams.WebsocketList.FirstOrDefault(ws => ws.Id == cnn.Id);

                if (webSocket is not null)
                {
                    if (webSocket.WebSocket.State == WebSocketState.Closed)
                    {
                        StaticParams.WebsocketList.Remove(webSocket);
                    }
                    else
                    {
                        var receiveWebSocketData = new ReceiveWebSocketData
                        {
                            UserId = cnn.UserId,
                            DataType = WebSocketDataType.IsError.ToString(),
                            ErrorMessage = errorMessage
                        };

                        var options = new JsonSerializerOptions
                        {
                            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                            WriteIndented = true
                        };
                        var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));
                        await webSocket.WebSocket.SendAsync(
                                new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                                WebSocketMessageType.Text,
                                true,
                                CancellationToken.None);

                    }
                }
            }
            catch (Exception)
            {
                if (cnn is not null)
                {
                    await CloseConnection(cnn);
                }
            }
        }

        private async Task JoinVideoCall(SendWebSocketData websocketData, WebSocketConnection cnn)
        {
            if (IsInVideoCall(cnn.UserId))
            {
                await SendErrorNotification(cnn, MsgNo.ERROR_USER_ALREADY_IN_CALL);
                return;
            }

            var videoCall = StaticParams.VideoCallList.FirstOrDefault(cr => cr.RoomId == websocketData.VideoCall.RoomId);
            if (videoCall is not null)
            {
                videoCall.Participants.TryAdd(cnn.UserId, websocketData.VideoCall.PeerId);
                await VideoCallSocket(websocketData, videoCall.RoomId, cnn);
            }
            else
            {
                videoCall = new VideoCallWebsocket
                {
                    RoomId = websocketData.VideoCall.RoomId,
                    Participants = new ConcurrentDictionary<string, string>
                    {
                        [cnn.UserId] = websocketData.VideoCall.PeerId
                    }
                };
                StaticParams.VideoCallList.Add(videoCall);
                websocketData.VideoCall.VideoCallType = MessageType.IsStartCall.ToString();

                await VideoCallSocket(websocketData, videoCall.RoomId, cnn);
            }
        }

        private async Task LeaveVideoCall(SendWebSocketData websocketData, WebSocketConnection cnn)
        {
            var videoCall = StaticParams.VideoCallList.FirstOrDefault(cr => cr.RoomId == websocketData.VideoCall.RoomId);
            if (videoCall is not null)
            {
                var peerId = videoCall.Participants[cnn.UserId];
                videoCall.Participants.TryRemove(cnn.UserId, out _);

                if (videoCall.Participants.IsEmpty)
                {
                    StaticParams.VideoCallList.Remove(videoCall);
                    websocketData.VideoCall.VideoCallType = SystemMessageType.IsEndCall.ToString();
                }

                await VideoCallSocket(websocketData, videoCall.RoomId, cnn, peerId);
            }
            else
            {
                await SendErrorNotification(cnn, MsgNo.ERROR_USER_NOT_IN_CALL);
            }
        }

        private async Task VideoCallSocket(SendWebSocketData websocketData, long roomId, WebSocketConnection cnn, string peerId = null)
        {
            var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(roomId);
            var listOfWebSocket = StaticParams.WebsocketList.Where(ws => listOfUserId.Contains(ws.UserId)).ToList();

            if (listOfWebSocket.Count > 0)
            {
                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                };

                var receiveWebSocketData = new ReceiveWebSocketData
                {
                    UserId = cnn.UserId,
                    DataType = websocketData.DataType,
                    VideoCall = new VideoCallData
                    {
                        RoomId = roomId,
                        VideoCallType = websocketData.VideoCall.VideoCallType,
                        PeerId = peerId ?? websocketData.VideoCall.PeerId,
                    }
                };
                var tasks = new List<Task>();

                foreach (WebSocketConnection webSocket in listOfWebSocket)
                {
                    if (webSocket.WebSocket.State == WebSocketState.Closed)
                    {
                        StaticParams.WebsocketList.Remove(webSocket);
                        continue;
                    }

                    if (webSocket.UserId == cnn.UserId)
                    {
                        receiveWebSocketData.VideoCall.Participants = await _videoCallService.GetParticipantInfosInCall(roomId);
                    }
                    else
                    {
                        receiveWebSocketData.VideoCall.Participants = await _videoCallService.GetUserInfoWhenJoinCall(roomId, cnn.UserId);
                    }
                    var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));
                    tasks.Add(webSocket.WebSocket.SendAsync(
                        new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None));
                }

                await Task.WhenAll(tasks);
            }

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
                await SendMessage(webSocketMessage, cnn);

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
                await SendSystemMessage(webSocketMessage, cnn, "", SystemMessageType.IsJoinCall.ToString());
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
                await SendSystemMessage(webSocketMessage, cnn, "", websocketData.VideoCall.VideoCallType);
            }
        }

        private async Task SendFriendRequest(SendWebSocketData websocketData, string userId)
        {
            var notification = await _notificationService.AddSendFriendRequestNotification(userId, websocketData.Notification.ReceiverId);
            var listOfWebSocket = StaticParams.WebsocketList.Where(ws => ws.UserId == websocketData.Notification.ReceiverId).ToList();

            if (listOfWebSocket.Count > 0)
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

                foreach (WebSocketConnection webSocket in listOfWebSocket)
                {
                    if (webSocket.WebSocket.State == WebSocketState.Closed)
                    {
                        StaticParams.WebsocketList.Remove(webSocket);
                        continue;
                    }

                    tasks.Add(webSocket.WebSocket.SendAsync(
                        new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None));
                }
                await Task.WhenAll(tasks);
            }
        }

        private async Task SendFriendRequestAcception(SendWebSocketData websocketData, string userId)
        {
            var notification = await _notificationService.AddAcceptedFriendRequestNotification(userId, websocketData.Notification.ReceiverId);
            var listOfWebSocket = StaticParams.WebsocketList
                .Where(ws => ws.UserId == websocketData.Notification.ReceiverId).ToList();

            if (listOfWebSocket.Count > 0)
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

                foreach (WebSocketConnection webSocket in listOfWebSocket)
                {
                    if (webSocket.WebSocket.State == WebSocketState.Closed)
                    {
                        StaticParams.WebsocketList.Remove(webSocket);
                        continue;
                    }

                    tasks.Add(webSocket.WebSocket.SendAsync(
                        new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None));
                }
                await Task.WhenAll(tasks);
            }
        }

        private async Task SendFriendRequestAcception(string userId)
        {
            var listOfWebSocket = StaticParams.WebsocketList.Where(ws => ws.UserId == userId).ToList();

            if (listOfWebSocket.Count > 0)
            {
                var receiveWebSocketData = new ReceiveWebSocketData
                {
                    UserId = userId,
                    DataType = WebSocketDataType.IsReloadChats.ToString(),
                };

                var options = new JsonSerializerOptions
                {
                    Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
                    WriteIndented = true
                };
                var serverMsg = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(receiveWebSocketData, options));
                var tasks = new List<Task>();

                foreach (WebSocketConnection webSocket in listOfWebSocket)
                {
                    if (webSocket.WebSocket.State == WebSocketState.Closed)
                    {
                        StaticParams.WebsocketList.Remove(webSocket);
                        continue;
                    }

                    tasks.Add(webSocket.WebSocket.SendAsync(
                        new ArraySegment<byte>(serverMsg, 0, serverMsg.Length),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None));
                }
                await Task.WhenAll(tasks);
            }
        }

        private async Task SendPostFileInClassRoom(SendWebSocketData websocketData, string userId)
        {
            var listOfUserId = await _roomService.GetListOfUserIdInRoomAsync(websocketData.Notification.RoomId);
            if (listOfUserId.Count > 0)
            {
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

                        var listOfWebSocket = StaticParams.WebsocketList.Where(ws => ws.UserId == receivedId).ToList();

                        foreach (WebSocketConnection webSocket in listOfWebSocket)
                        {
                            if (webSocket.WebSocket.State == WebSocketState.Closed)
                            {
                                StaticParams.WebsocketList.Remove(webSocket);
                                continue;
                            }

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
        }

        private static bool IsInVideoCall(string userId)
        {
            return StaticParams.VideoCallList.Any(cr => cr.Participants.ContainsKey(userId));
        }
    }
}