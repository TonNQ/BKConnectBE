using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model;
using BKConnectBE.Model.Dtos.RoomManagement;
using BKConnectBE.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKConnectBE.Repository.Rooms
{
    public class RoomRepository : IRoomRepository
    {
        private readonly BKConnectContext _context;

        public RoomRepository(BKConnectContext context)
        {
            _context = context;
        }

        public async Task CreateNewPrivateRoom(string userId1, string userId2)
        {
            await _context.AddAsync(new Room()
            {
                RoomType = RoomType.PrivateRoom.ToString(),
                Name = null,
                Avatar = null,
                UsersOfRoom = new List<UserOfRoom>()
                {
                    new ()
                    {
                        UserId = userId1,
                        ReadMessageId = null
                    },
                    new ()
                    {
                        UserId = userId2,
                        ReadMessageId = null
                    }
                },
                Messages = new List<Message>(){
                    new ()
                    {
                        TypeOfMessage = MessageType.Text.ToString(),
                        Content = Constants.FRIEND_ACCEPTED_NOTIFICATION,
                        SendTime = DateTime.Now,
                        SenderId = null
                    }
                }
            });
        }

        public async Task<List<RoomSidebarDto>> GetListOfRoomsByUserId(string userId, string searchKey)
        {
            return await _context.Rooms
                .Where(r => r.UsersOfRoom.Any(u => u.UserId == userId)
                    && ((r.RoomType != RoomType.PrivateRoom.ToString() && r.Name.Contains(searchKey))
                    || (r.RoomType == RoomType.PrivateRoom.ToString()
                    && r.UsersOfRoom.Any(u => u.UserId != userId && u.User.Name.Contains(searchKey)))))
                .Join(_context.Messages.Where(m => m.Id == m.Room.Messages.OrderBy(x => x.Id).LastOrDefault().Id), r => r.Id, m => m.RoomId, (room, message) => new { room, message })
                .Select(result => new RoomSidebarDto()
                {
                    Id = result.room.Id,
                    RoomType = result.room.RoomType,
                    Name = result.room.RoomType == RoomType.PrivateRoom.ToString() ? result.room.UsersOfRoom.FirstOrDefault(u => u.UserId != userId).User.Name : result.room.Name,
                    Avatar = result.room.RoomType == RoomType.PrivateRoom.ToString() ? result.room.UsersOfRoom.FirstOrDefault(u => u.UserId != userId).User.Avatar : result.room.Avatar,
                    IsRead = result.room.UsersOfRoom.FirstOrDefault(u => u.UserId == userId).ReadMessage != null
                        && !result.room.Messages.Any(m => m.SenderId != userId
                            && m.Id > result.room.UsersOfRoom.FirstOrDefault(u => u.UserId == userId).ReadMessageId),
                    LastMessageTime = result.message.SendTime,
                    LastMessage = (result.message.SenderId == null ? ""
                        : ((result.message.SenderId == userId
                            ? "Bạn: " : $"{result.message.Sender.Name}: ")))
                        + (result.message.TypeOfMessage == MessageType.Text.ToString()
                            ? result.message.Content
                            : $"Đã gửi một {Helper.GetEnumDescription(result.message.TypeOfMessage.ToEnum<MessageType>())}"),
                })
                .OrderByDescending(r => r.LastMessageTime)
                .ThenByDescending(r => r.Id)
                .ToListAsync();
        }

        public async Task<List<string>> GetListOfUserIdInRoomAsync(long roomId)
        {
            var room = await _context.Rooms.Include(r => r.UsersOfRoom).FirstOrDefaultAsync(r => r.Id == roomId) ?? throw new Exception(MsgNo.ERROR_ROOM_NOT_FOUND); ;
            return room.UsersOfRoom.Select(u => u.UserId).ToList();
        }
    }
};