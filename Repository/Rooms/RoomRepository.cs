using BKConnect.BKConnectBE.Common;
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
            return await _context.Rooms.Include(r => r.UsersOfRoom)
                .Where(r => r.UsersOfRoom.Any(u => u.UserId == userId && !u.IsDeleted)).ToListAsync();
        }

        public async Task<List<string>> GetListOfUserIdInRoomAsync(long roomId)
        {
            var room = await _context.Rooms.Include(r => r.UsersOfRoom).FirstOrDefaultAsync(r => r.Id == roomId)
                ?? throw new Exception(MsgNo.ERROR_ROOM_NOT_FOUND);
            return room.UsersOfRoom.Where(u => !u.IsDeleted).Select(u => u.UserId).ToList();
        }
        public async Task<List<string>> GetListOfOldUserIdInRoomAsync(long roomId, List<string> newUserId)
        {
            var room = await _context.Rooms.Include(r => r.UsersOfRoom).FirstOrDefaultAsync(r => r.Id == roomId)
                ?? throw new Exception(MsgNo.ERROR_ROOM_NOT_FOUND);
            return room.UsersOfRoom.Where(u => !u.IsDeleted && !newUserId.Contains(u.UserId)).Select(u => u.UserId).ToList();
        }

        public async Task<List<UserOfRoom>> GetListOfMembersInRoomAsync(long roomId, string userId)
        {
            var room = await _context.Rooms.Include(r => r.UsersOfRoom).ThenInclude(u => u.User)
                .FirstOrDefaultAsync(r => r.Id == roomId && r.UsersOfRoom.Any(u => u.UserId == userId))
                ?? throw new Exception(MsgNo.ERROR_ROOM_NOT_FOUND);
            return room.UsersOfRoom.Where(u => !u.IsDeleted).ToList();
        }

        public async Task<UserOfRoom> GetUserOfRoomInfo(long roomId, string userId)
        {
            var member = await _context.UsersOfRoom.Include(m => m.User)
            .FirstOrDefaultAsync(m => m.RoomId == roomId && m.UserId == userId);
            return member;
        }

        public async Task<User> GetFriendInRoom(long roomId, string userId)
        {
            var member = await _context.UsersOfRoom.Include(m => m.User)
            .FirstOrDefaultAsync(m => m.RoomId == roomId && m.UserId != userId);
            return member?.User;
        }

        public async Task<List<Room>> GetListOfRoomsByTypeAndUserId(string type, string userId)
        {
            return await _context.Rooms.Include(r => r.UsersOfRoom)
                .Where(r => r.UsersOfRoom.Any(u => u.UserId == userId && !u.IsDeleted) && r.RoomType == type)
                .ToListAsync();
        }

        public async Task<int?> GetTotalMemberOfRoom(long roomId)
        {
            var usersOfRoom = await _context.UsersOfRoom.Where(u => u.RoomId == roomId && !u.IsDeleted).ToListAsync();
            return usersOfRoom.Count;
        }

        public async Task<bool> IsInRoomAsync(long roomId, string userId)
        {
            return await _context.UsersOfRoom.AnyAsync(u => u.UserId == userId && u.RoomId == roomId && !u.IsDeleted);
        }

        public async Task<bool> IsExistBefore(long roomId, string userId)
        {
            return await _context.UsersOfRoom.AnyAsync(u => u.UserId == userId && u.RoomId == roomId);
        }

        public async Task<bool> IsAdmin(long roomId, string userId)
        {
            var member = await _context.UsersOfRoom.FirstOrDefaultAsync(u => u.UserId == userId && u.RoomId == roomId && !u.IsDeleted)
            ?? throw new Exception(MsgNo.ERROR_USER_NOT_IN_ROOM);
            return member.IsAdmin;
        }

        public async Task RemoveUserById(long roomId, string userId)
        {
            var member = await _context.UsersOfRoom.FirstOrDefaultAsync(m => m.RoomId == roomId && m.UserId == userId)
                ?? throw new Exception(MsgNo.ERROR_USER_NOT_IN_ROOM);
            member.IsDeleted = true;
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAvatar(long roomId, string img)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId)
                ?? throw new Exception(MsgNo.ERROR_ROOM_NOT_FOUND);
            room.Avatar = img;
            await _context.SaveChangesAsync();
        }

        public async Task UpdateName(long roomId, string name)
        {
            var room = await _context.Rooms.FirstOrDefaultAsync(r => r.Id == roomId)
                ?? throw new Exception(MsgNo.ERROR_ROOM_NOT_FOUND);
            room.Name = name;
            await _context.SaveChangesAsync();
        }

        public async Task SetReadMessageOfRoom(string userId, ReadMessageOfRoomDto readMessage)
        {
            var userOfRoom = await _context.UsersOfRoom
                .FirstOrDefaultAsync(u => u.UserId == userId && u.RoomId == readMessage.RoomId && !u.IsDeleted)
                ?? throw new Exception(MsgNo.ERROR_USER_NOT_IN_ROOM);
            if (!_context.Messages.Any(m => m.Id == readMessage.MessageId && m.RoomId == readMessage.RoomId))
            {
                throw new Exception(MsgNo.ERROR_MESSAGE_NOT_IN_ROOM);
            }
            else
            {
                userOfRoom.ReadMessageId = readMessage.MessageId;
            }
        }
    }
};