using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model.Dtos.RoomManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository.Rooms;

namespace BKConnectBE.Service.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
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
                        roomDto.LastMessage = (lastMessage.SenderId == null ? ""
                            : (lastMessage.SenderId == userId ? "Bạn: " : $"{lastMessage.Sender.Name}: "))
                            + (lastMessage.TypeOfMessage == nameof(MessageType.Text)
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
                        roomDto.IsRead = user.ReadMessage != null
                            && !room.Messages.Any(m => m.SenderId != userId
                                && m.Id > user.ReadMessageId);
                        roomDto.LastMessageTime = lastMessage.SendTime;
                        roomDto.LastMessage = (lastMessage.SenderId == null ? ""
                            : (lastMessage.SenderId == userId ? "Bạn: " : $"{lastMessage.Sender.Name}: "))
                            + (lastMessage.TypeOfMessage == nameof(MessageType.Text)
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
    }
}