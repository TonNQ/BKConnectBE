using BKConnectBE.Model;
using BKConnectBE.Model.Dtos.FriendRequestManagement;
using BKConnectBE.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKConnectBE.Repository.FriendRequests
{
    public class FriendRequestRepository : IFriendRequestRepository
    {
        private readonly BKConnectContext _context;

        public FriendRequestRepository(BKConnectContext context)
        {
            _context = context;
        }

        public Task<List<ReceivedFriendRequestDto>> GetListOfReceivedFriendRequestsOfUser(string userId)
        {
            return _context.FriendRequests
                .Include(fr => fr.Sender)
                .ThenInclude(sd => sd.Class)
                .Where(fr => fr.ReceiverId == userId)
                .OrderByDescending(fr => fr.SendTime)
                .Select(fr => new ReceivedFriendRequestDto()
                {
                    Id = fr.Id,
                    SendTime = fr.SendTime,
                    SenderId = fr.SenderId,
                    SenderName = fr.Sender.Name,
                    SenderAvatar = fr.Sender.Avatar,
                    SenderClassName = fr.Sender.Class.Name
                }).ToListAsync();
        }

        public async Task<bool> CheckFriendRequest(string senderId, string receiverId)
        {
            return await _context.FriendRequests.AnyAsync(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId);
        }

        public async Task<bool> CheckCanSendFriendRequest(string senderId, string receiverId)
        {
            var listId = new List<string>{
                senderId,
                receiverId
            };

            if (senderId == receiverId || !await _context.Users.AnyAsync(u => listId.Contains(u.Id)))
            {
                return false;
            }

            return !await _context.FriendRequests.AnyAsync(fr => listId.Contains(fr.ReceiverId) && listId.Contains(fr.SenderId))
                && !await _context.Relationships.AnyAsync(r => listId.Contains(r.User1Id) && listId.Contains(r.User2Id));
        }

        public async Task CreateFriendRequest(string senderId, string receiverId)
        {
            await _context.FriendRequests.AddAsync(new FriendRequest()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                SendTime = DateTime.Now,
            });
        }

        public async Task<FriendRequest> GetFriendRequestByUser(string user1Id, string user2Id)
        {
            var list = new List<string> { user1Id, user2Id };

            return await _context.FriendRequests
                .Include(fr => fr.Sender)
                .ThenInclude(sd => sd.Class)
                .FirstOrDefaultAsync(fr => list.Contains(fr.SenderId) && list.Contains(fr.ReceiverId));
        }
    }
}