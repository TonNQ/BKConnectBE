using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.MessageManagement;
using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Model.Dtos.RoomManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Rooms;
using BKConnectBE.Repository.Users;
using BKConnectBE.Service.Messages;

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

        public async Task<List<RoomDetailDto>> GetListOfRoomsByUserId(string userId, SearchKeyCondition? condition)
        {
            condition ??= new SearchKeyCondition();
            var searchKey = Helper.RemoveUnicodeSymbol(condition.SearchKey);
            var rooms = await _roomRepository.GetListOfRoomsByUserId(userId);
            var roomDtos = new List<RoomDetailDto>();

            foreach (Room room in rooms)
            {
                var lastMessage = room.Messages.OrderByDescending(m => m.SendTime).FirstOrDefault()
                    ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);
                var roomDto = _mapper.Map<RoomDetailDto>(room);

                if (room.RoomType != nameof(RoomType.PrivateRoom) &&
                    Helper.RemoveUnicodeSymbol(room.Name).Contains(searchKey))
                {
                    var user = room.UsersOfRoom.FirstOrDefault(u => u.UserId == userId)
                        ?? throw new Exception(MsgNo.ERROR_INTERNAL_SERVICE);

                    roomDto.IsRead = user.ReadMessage != null
                            && !room.Messages.Any(m => m.SenderId != userId
                                && m.Id > user.ReadMessageId);
                    roomDto.LastMessageTime = lastMessage.SendTime;

                    roomDto.LastMessage = lastMessage.TypeOfMessage == nameof(MessageType.System)
                        ? await _messageService.ChangeContentSystemMessage(lastMessage.Id, userId)
                        : ((lastMessage.SenderId == userId ? "Bạn: " : $"{lastMessage.Sender.Name}: ")
                            + (lastMessage.TypeOfMessage == nameof(MessageType.Text)
                                ? lastMessage.Content
                                : $"Đã gửi một {Helper.GetEnumDescription(lastMessage.TypeOfMessage.ToEnum<MessageType>())}"));
                    roomDtos.Add(roomDto);
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
                        roomDto.FriendId = friend.Id;

                        if (StaticParams.WebsocketList.Any(w => w.UserId == friend.Id))
                        {
                            roomDto.IsOnline = true;
                        }
                        else
                        {
                            roomDto.LastOnline = friend.LastOnline;
                        }

                        roomDto.IsRead = user.ReadMessage != null
                            && !room.Messages.Any(m => m.SenderId != userId
                                && m.Id > user.ReadMessageId);
                        roomDto.LastMessageTime = lastMessage.SendTime;
                        roomDto.LastMessage = lastMessage.TypeOfMessage == nameof(MessageType.System)
                            ? await _messageService.ChangeContentSystemMessage(lastMessage.Id, userId)
                            : ((lastMessage.SenderId == userId ? "Bạn: " : $"{lastMessage.Sender.Name}: ")
                                + (lastMessage.TypeOfMessage == nameof(MessageType.Text)
                                    ? lastMessage.Content
                                    : $"Đã gửi một {Helper.GetEnumDescription(lastMessage.TypeOfMessage.ToEnum<MessageType>())}"));
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
        public async Task<List<string>> GetListOfOldUserIdInRoomAsync(long roomId, List<string> newUserId)
        {
            return await _roomRepository.GetListOfOldUserIdInRoomAsync(roomId, newUserId);
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

        public async Task<RoomDetailDto> CreateGroupRoomAsync(AddGroupRoomDto addGroupRoomDto, string userId)
        {
            if (addGroupRoomDto.UserIds.Count < 2
                || addGroupRoomDto.RoomType == RoomType.PrivateRoom.ToString())
            {
                throw new Exception(MsgNo.ERROR_CREATE_GROUP_ROOM);
            }

            var room = _mapper.Map<Room>(addGroupRoomDto);
            var msg = new Message()
            {
                SenderId = userId,
                RoomId = room.Id,
                TypeOfMessage = MessageType.System.ToString(),
                Content = SystemMessageType.IsCreateGroupRoom.ToString(),
                SendTime = DateTime.UtcNow.AddHours(7)
            };
            room.Messages.Add(msg);

            await _userRepository.GetByIdAsync(userId);
            room.UsersOfRoom.Add(new UserOfRoom
            {
                IsAdmin = true,
                UserId = userId,
                RoomId = room.Id,
                JoinTime = DateTime.UtcNow.AddHours(7)
            });

            foreach (var id in addGroupRoomDto.UserIds)
            {
                await _userRepository.GetByIdAsync(id);
                room.UsersOfRoom.Add(new UserOfRoom
                {
                    IsAdmin = false,
                    UserId = id,
                    RoomId = room.Id,
                    JoinTime = DateTime.UtcNow.AddHours(7)
                });
            };

            await _genericRepositoryForRoom.AddAsync(room);
            await _genericRepositoryForUserOfRoom.SaveChangeAsync();

            var roomDto = _mapper.Map<RoomDetailDto>(room);
            roomDto.LastMessageId = msg.Id;
            roomDto.LastMessageTime = msg.SendTime.AddHours(-7);
            return roomDto;
        }

        private async Task<SendMessageDto> AddingUserToRoomAsync(long roomId, string addedUserId, string userId)
        {
            string[] ids = addedUserId.Split(", ");
            var tasks = new List<Task>();
            for (int i = 0; i < ids.Length; i++)
            {
                if (await _roomRepository.IsExistBefore(roomId, ids[i]))
                {
                    var userInfo = await _roomRepository.GetUserOfRoomInfo(roomId, ids[i]);
                    userInfo.IsDeleted = false;
                }
                else
                {
                    var member = new UserOfRoom
                    {
                        IsAdmin = false,
                        UserId = ids[i],
                        RoomId = roomId,
                        JoinTime = DateTime.UtcNow.AddHours(7)
                    };
                    tasks.Add(_genericRepositoryForUserOfRoom.AddAsync(member));
                }
            }
            await Task.WhenAll(tasks);

            await _genericRepositoryForUserOfRoom.SaveChangeAsync();

            var addMsg = new SendMessageDto
            {
                RoomId = roomId,
                TypeOfMessage = MessageType.System.ToString(),
                Content = SystemMessageType.IsInRoom.ToString()
            };

            return addMsg;
        }

        public async Task<SendMessageDto> RemoveUserFromRoom(long roomId, string removeId, string userId)
        {
            if (!await _roomRepository.IsAdmin(roomId, userId))
            {
                throw new Exception(MsgNo.ERROR_UNHADLED_ACTION);
            }

            if (!await _roomRepository.IsInRoomAsync(roomId, removeId))
            {
                throw new Exception(MsgNo.ERROR_USER_NOT_IN_ROOM);
            }

            await _roomRepository.RemoveUserById(roomId, removeId);

            var removeMsg = new SendMessageDto
            {
                RoomId = roomId,
                TypeOfMessage = MessageType.System.ToString(),
                Content = SystemMessageType.IsOutRoom.ToString()
            };

            return removeMsg;
        }

        public async Task<SendMessageDto> LeaveRoom(long roomId, string userId)
        {
            if (!await _roomRepository.IsInRoomAsync(roomId, userId))
            {
                throw new Exception(MsgNo.ERROR_USER_NOT_IN_ROOM);
            }

            await _roomRepository.RemoveUserById(roomId, userId);

            var leaveMsg = new SendMessageDto
            {
                RoomId = roomId,
                TypeOfMessage = MessageType.System.ToString(),
                Content = SystemMessageType.IsLeaveRoom.ToString()
            };

            return leaveMsg;
        }

        public async Task<ChangedRoomDto> GetChangedRoomInfo(long roomId, string affectedId, bool isAdd = true)
        {
            var changedRoomInfo = new ChangedRoomDto
            {
                RoomId = roomId,
                TotalMember = await _roomRepository.GetTotalMemberOfRoom(roomId)
            };
            string[] ids = affectedId.Split(", ");
            if (!isAdd)
            {
                changedRoomInfo.LeftMemberId = ids[0];
            }
            else
            {
                changedRoomInfo.NewMemberList = new List<MemberOfRoomDto>();

                for (int i = 0; i < ids.Length; i++)
                {
                    var newMember = await _roomRepository.GetUserOfRoomInfo(roomId, ids[i]);
                    changedRoomInfo.NewMemberList.Add(_mapper.Map<MemberOfRoomDto>(newMember));
                }
            }
            return changedRoomInfo;
        }

        public async Task<RoomDetailDto> GetRoomInformation(long roomId)
        {
            return  _mapper.Map<RoomDetailDto>(await _roomRepository.GetInformationOfRoom(roomId));
        }
    }
}