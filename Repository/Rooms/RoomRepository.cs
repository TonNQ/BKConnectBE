using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model;
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

        public async Task CreateNewPrivateRoom(string userId1, string userId2, string serverMessage)
        {
            var room = await _context.Rooms
                .FirstOrDefaultAsync(r => r.RoomType == RoomType.PrivateRoom.ToString() &&
                    r.UsersOfRoom.Any(u => u.UserId == userId1) &&
                    r.UsersOfRoom.Any(u => u.UserId == userId2));

            if (room is null)
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
                            JoinTime = DateTime.UtcNow.AddHours(7),
                            ReadMessageId = null
                        },
                        new ()
                        {
                            UserId = userId2,
                            JoinTime = DateTime.UtcNow.AddHours(7),
                            ReadMessageId = null
                        }
                    },
                    Messages = new List<Message>()
                    {
                        new ()
                        {
                            TypeOfMessage = MessageType.System.ToString(),
                            Content = serverMessage,
                            SendTime = DateTime.UtcNow.AddHours(7),
                            SenderId = null
                        }
                    },
                    CreatedDate = DateTime.UtcNow.AddHours(7),
                    UpdatedDate = DateTime.UtcNow.AddHours(7)
                });
            }
        }

        public async Task<Room> GetInformationOfRoom(long roomId)
        {
            return await _context.Rooms
                .Include(r => r.UsersOfRoom).ThenInclude(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<List<Room>> GetListOfRoomsByUserId(string userId)
        {
            return await _context.Rooms
                .Include(m => m.Messages)
                .Include(r => r.UsersOfRoom).ThenInclude(u => u.User)
                .Where(r => r.UsersOfRoom.Any(u => u.UserId == userId)).ToListAsync();

        }

        public async Task<List<string>> GetListOfUserIdInRoomAsync(long roomId)
        {
            var room = await _context.Rooms.Include(r => r.UsersOfRoom).FirstOrDefaultAsync(r => r.Id == roomId)
                ?? throw new Exception(MsgNo.ERROR_ROOM_NOT_FOUND); ;
            return room.UsersOfRoom.Select(u => u.UserId).ToList();
        }

        public async Task<List<UserOfRoom>> GetListOfMembersInRoomAsync(long roomId, string userId)
        {
            var room = await _context.Rooms.Include(r => r.UsersOfRoom).ThenInclude(u => u.User)
                .FirstOrDefaultAsync(r => r.Id == roomId && r.UsersOfRoom.Any(u => u.UserId == userId))
                ?? throw new Exception(MsgNo.ERROR_ROOM_NOT_FOUND); ;
            return room.UsersOfRoom.ToList();
        }

        public async Task<List<Room>> GetListOfRoomsByTypeAndUserId(string type, string userId)
        {
            return await _context.Rooms.Include(r => r.UsersOfRoom)
                .Where(r => r.UsersOfRoom.Any(u => u.UserId == userId) && r.RoomType == type)
                .ToListAsync();
        }

        public async Task<bool> IsInRoomAsync(long roomId, string userId)
        {
            return await _context.UsersOfRoom.AnyAsync(u => u.UserId == userId && u.RoomId == roomId);
        }

        public async Task<UserOfRoom> GetAnUserOfRoom(long roomId, string userId)
        {
            return await _context.UsersOfRoom.FirstOrDefaultAsync(m => m.RoomId == roomId && m.UserId == userId) 
                ?? throw new Exception(MsgNo.ERROR_USER_NOT_IN_ROOM);
        }
    }
};