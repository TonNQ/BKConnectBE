using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.ChatManagement;
using BKConnectBE.Model.Dtos.MessageManagement;
using BKConnectBE.Model.Dtos.NotificationManagement;
using BKConnectBE.Model.Dtos.RoomManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Messages;
using BKConnectBE.Repository.Rooms;
using BKConnectBE.Repository.Users;
using BKConnectBE.Service.Messages;
using BKConnectBE.Service.WebSocket;

namespace BKConnectBE.Service.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IUserRepository _userRepository;
        private readonly IGenericRepository<Room> _genericRepositoryForRoom;
        private readonly IGenericRepository<UserOfRoom> _genericRepositoryForUserOfRoom;
        private readonly IMessageService _messageService;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IUserRepository userRepository,
            IMessageService messageService,
            IGenericRepository<Room> genericRepositoryForRoom,
            IGenericRepository<UserOfRoom> genericRepositoryForUserOfRoom,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _userRepository = userRepository;
            _messageService = messageService;
            _genericRepositoryForRoom = genericRepositoryForRoom;
            _genericRepositoryForUserOfRoom = genericRepositoryForUserOfRoom;
            _mapper = mapper;
        }

        public async Task<RoomDetailDto> GetInformationOfRoom(long roomId, string userId)
        {
            var room = await _roomRepository.GetInformationOfRoom(roomId);
            if (!room.UsersOfRoom.Any(u => u.UserId == userId))
            {
                throw new Exception(MsgNo.ERROR_UNAUTHORIZED);
            }

            var roomDto = _mapper.Map<RoomDetailDto>(room);
            if (room.RoomType == nameof(RoomType.PrivateRoom))
            {
                var friend = room.UsersOfRoom.FirstOrDefault(u => u.UserId != userId).User
                    ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);

                roomDto.Name = friend.Name;
                roomDto.Avatar = friend.Avatar;
                if (WebSockets.WebsocketList.Any(w => w.UserId == friend.Id))
                {
                    roomDto.IsOnline = true;
                }
                else
                {
                    roomDto.LastOnline = friend.LastOnline;
                }
            }
            else
            {
                roomDto.TotalMember = room.UsersOfRoom.Count;
            }
            return roomDto;
        }

        public async Task<List<RoomSidebarDto>> GetListOfRoomsByUserId(string userId, string searchKey = "")
        {
            searchKey = Helper.RemoveUnicodeSymbol(searchKey);
            var rooms = await _roomRepository.GetListOfRoomsByUserId(userId);
            var roomDtos = new List<RoomSidebarDto>();

            foreach (Room room in rooms)
            {
                var lastMessage = room.Messages.OrderByDescending(m => m.SendTime).FirstOrDefault()
                    ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);
                var roomDto = _mapper.Map<RoomSidebarDto>(room);
                if (room.RoomType != nameof(RoomType.PrivateRoom) && Helper.RemoveUnicodeSymbol(room.Name).Contains(searchKey))
                {
                    var user = room.UsersOfRoom.FirstOrDefault(u => u.UserId == userId)
                        ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);

                    if (Helper.RemoveUnicodeSymbol(room.Name).Contains(searchKey))
                    {
                        roomDto.IsRead = user.ReadMessage != null
                            && !room.Messages.Any(m => m.SenderId != userId
                                && m.Id > user.ReadMessageId);
                        roomDto.LastMessageTime = lastMessage.SendTime;

                        var textType = new List<string> { nameof(MessageType.Text), nameof(MessageType.System) };
                        roomDto.LastMessage = (lastMessage.SenderId == null || lastMessage.TypeOfMessage == MessageType.System.ToString() ? ""
                            : (lastMessage.SenderId == userId ? "Bạn: " : $"{lastMessage.Sender.Name}: "))
                            + (textType.Contains(lastMessage.TypeOfMessage)
                                ? lastMessage.Content
                                : $"Đã gửi một {Helper.GetEnumDescription(lastMessage.TypeOfMessage.ToEnum<MessageType>())}");
                        roomDtos.Add(roomDto);
                    }
                }
                else if (room.RoomType == nameof(RoomType.PrivateRoom))
                {
                    var friend = room.UsersOfRoom.FirstOrDefault(u => u.UserId != userId).User
                        ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);
                    var user = room.UsersOfRoom.FirstOrDefault(u => u.UserId == userId)
                        ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);

                    if (Helper.RemoveUnicodeSymbol(friend.Name).Contains(searchKey))
                    {
                        roomDto.Name = friend.Name;
                        roomDto.Avatar = friend.Avatar;
                        if (WebSockets.WebsocketList.Any(w => w.UserId == friend.Id))
                        {
                            roomDto.IsOnline = true;
                        }

                        var textType = new List<string> { nameof(MessageType.Text), nameof(MessageType.System) };
                        roomDto.IsRead = user.ReadMessage != null
                            && !room.Messages.Any(m => m.SenderId != userId
                                && m.Id > user.ReadMessageId);
                        roomDto.LastMessageTime = lastMessage.SendTime;
                        roomDto.LastMessage = (lastMessage.SenderId == null || lastMessage.TypeOfMessage == MessageType.System.ToString() ? ""
                            : (lastMessage.SenderId == userId ? "Bạn: " : $"{lastMessage.Sender.Name}: "))
                            + (textType.Contains(lastMessage.TypeOfMessage)
                                ? lastMessage.Content
                                : $"Đã gửi một {Helper.GetEnumDescription(lastMessage.TypeOfMessage.ToEnum<MessageType>())}");
                        roomDtos.Add(roomDto);
                    }
                }
            }
            return roomDtos.OrderByDescending(r => r.LastMessageTime).ToList();
        }

        public async Task<List<string>> GetListOfUserIdInRoomAsync(long roomId)
        {
            return await _roomRepository.GetListOfUserIdInRoomAsync(roomId);
        }

        public async Task<List<MemberOfRoomDto>> GetListOfMembersInRoomAsync(long roomId, string userId)
        {
            var members = await _roomRepository.GetListOfMembersInRoomAsync(roomId, userId);
            return _mapper.Map<List<MemberOfRoomDto>>(members);
        }

        public async Task<List<GroupRoomDto>> GetListOfRoomsByTypeAndUserId(string type, string userId)
        {
            var rooms = await _roomRepository.GetListOfRoomsByTypeAndUserId(type, userId);
            var roomDtos = new List<GroupRoomDto>();

            foreach (Room room in rooms)
            {
                var user = room.UsersOfRoom.FirstOrDefault(u => u.UserId == userId)
                    ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);
                var roomDto = _mapper.Map<GroupRoomDto>(room);
                roomDto.JoinTime = user.JoinTime;
                roomDtos.Add(roomDto);
            }
            return roomDtos;
        }

        public async Task<List<GroupRoomDto>> SearchListOfRoomsByTypeAndUserId(string type, string userId, string searchKey = "")
        {
            var rooms = await _roomRepository.GetListOfRoomsByTypeAndUserId(type, userId);
            var roomDtos = new List<GroupRoomDto>();
            searchKey = Helper.RemoveUnicodeSymbol(searchKey);

            foreach (Room room in rooms)
            {
                if (Helper.RemoveUnicodeSymbol(room.Name).Contains(searchKey))
                {
                    var user = room.UsersOfRoom.FirstOrDefault(u => u.UserId == userId)
                        ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);
                    var roomDto = _mapper.Map<GroupRoomDto>(room);
                    roomDto.JoinTime = user.JoinTime;
                    roomDtos.Add(roomDto);
                }
            }
            return roomDtos;
        }

        public async Task<SendMessageDto> AddUserToRoomAsync(long roomId, string addedUserId, string userId)
        {
            if (!await _roomRepository.IsInRoomAsync(roomId, userId))
            {
                throw new Exception(MsgNo.ERROR_USER_NOT_IN_ROOM);
            }

            if (await _roomRepository.IsInRoomAsync(roomId, addedUserId))
            {
                throw new Exception(MsgNo.ERROR_USER_ALREADY_IN_ROOM);
            }

            var room = await _genericRepositoryForRoom.GetByIdAsync(roomId) ?? throw new Exception(MsgNo.ERROR_ROOM_NOT_FOUND);

            if (room.RoomType == RoomType.PrivateRoom.ToString())
            {
                throw new Exception(MsgNo.ERROR_ADD_USER_TO_ROOM);
            }
            else if (room.RoomType == RoomType.ClassRoom.ToString())
            {
                if (!await _userRepository.IsLecturer(userId))
                {
                    throw new Exception(MsgNo.ERROR_ADD_USER_TO_ROOM);
                }

                return await AddingUserToRoomAsync(roomId, addedUserId, userId);
            }
            else
            {
                return await AddingUserToRoomAsync(roomId, addedUserId, userId);
            }

            
        }

        private async Task<SendMessageDto> AddingUserToRoomAsync(long roomId, string addedUserId, string userId)
        {
            var member = new UserOfRoom
            {
                IsAdmin = false,
                UserId = addedUserId,
                RoomId = roomId
            };
            await _genericRepositoryForUserOfRoom.AddAsync(member);
            await _genericRepositoryForUserOfRoom.SaveChangeAsync();

            var addUsername = await _userRepository.GetUsernameById(addedUserId);
            var username = await _userRepository.GetUsernameById(userId);

            var addMsg = new SendMessageDto {
                RoomId = roomId,
                TypeOfMessage = MessageType.System.ToString(),
                Content = username + " đã thêm " + addUsername + " vào nhóm"
            };

            await _messageService.AddMessageAsync(addMsg, userId);

            return addMsg;
        }

        public async Task<SendMessageDto> RemoveUserFromRoom(long roomId, string removeId, string userId)
        {
            if (!await _roomRepository.IsInRoomAsync(roomId, removeId)) 
            {
                throw new Exception(MsgNo.ERROR_USER_NOT_IN_ROOM);
            }

            var member = await _roomRepository.GetAnUserOfRoom(roomId, removeId);
            await _genericRepositoryForUserOfRoom.RemoveByIdAsync(member.Id);
            await _genericRepositoryForUserOfRoom.SaveChangeAsync();

            var removeUsername = await _userRepository.GetUsernameById(removeId);
            var username = await _userRepository.GetUsernameById(userId);

            var removeMsg = new SendMessageDto {
                RoomId = roomId,
                TypeOfMessage = MessageType.System.ToString(),
                Content = username + " đã xoá " + removeUsername + " ra khỏi nhóm"
            };

            await _messageService.AddMessageAsync(removeMsg, userId);

            return removeMsg;
        }
    }
}