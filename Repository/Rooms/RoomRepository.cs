using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model;
using BKConnectBE.Model.Dtos.RoomManagement;
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

        public async Task<List<RoomSidebarDto>> GetListOfRoomsByUserId(string userId, string searchKey)
        {
            return await _context.Rooms
                .Include(r => r.UsersOfRoom)
                .ThenInclude(u => u.User)
                .Where(r => (r.Name.Contains(searchKey)
                    || (r.RoomType == RoomType.PrivateRoom.ToString()
                    && r.UsersOfRoom.Any(u => u.UserId != userId && u.User.Name.Contains(searchKey))))
                    && r.UsersOfRoom.Any(u => u.UserId == userId))
                .Select(r => new RoomSidebarDto()
                {
                    Id = r.Id,
                    RoomType = r.RoomType,
                    Name = r.RoomType == RoomType.PrivateRoom.ToString() ? r.UsersOfRoom.FirstOrDefault(u => u.UserId != userId).User.Name : r.Name,
                    Avatar = r.RoomType == RoomType.PrivateRoom.ToString() ? r.UsersOfRoom.FirstOrDefault(u => u.UserId != userId).User.Avatar : r.Avatar,
                    LastMessage = r.Messages.OrderByDescending(m => m.SendTime).FirstOrDefault() == null
                        ? "" : r.Messages.OrderByDescending(m => m.SendTime).FirstOrDefault().Content,
                    LastMessageTime = r.Messages.OrderByDescending(m => m.SendTime).FirstOrDefault() == null
                        ? DateTime.MinValue : r.Messages.OrderByDescending(m => m.SendTime).FirstOrDefault().SendTime,
                })
                .OrderByDescending(r => r.LastMessageTime)
                .ThenByDescending(r => r.Id)
                .ToListAsync();
        }
    }
}