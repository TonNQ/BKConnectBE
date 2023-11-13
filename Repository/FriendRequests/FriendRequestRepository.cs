using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
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

        public async Task<List<SentFriendRequestDto>> GetListOfAcceptedFriendRequestsOfUser(string userId)
        {
            return await _context.FriendRequests
                .Include(fr => fr.Receiver)
                .ThenInclude(r => r.Class)
                .Where(fr => fr.SenderId == userId && fr.Status == FriendRequestStatus.Accepted.ToString())
                .OrderByDescending(fr => fr.AcceptedTime)
                .Select(fr => new SentFriendRequestDto()
                {
                    Id = fr.Id,
                    SendTime = fr.SendTime,
                    Status = (int)fr.Status.ToEnum<FriendRequestStatus>(),
                    AcceptedTime = fr.AcceptedTime,
                    ReceiverId = fr.ReceiverId,
                    ReceiverName = fr.Receiver.Name,
                    ReceiverAvatar = fr.Receiver.Avatar,
                    ReceiverClassName = fr.Receiver.Class.Name
                }).ToListAsync();
        }

        public Task<List<ReceivedFriendRequestDto>> GetListOfReceivedFriendRequestsOfUser(string userId)
        {
            return _context.FriendRequests
                .Include(fr => fr.Sender)
                .ThenInclude(sd => sd.Class)
                .Where(fr => fr.ReceiverId == userId && fr.Status != FriendRequestStatus.Accepted.ToString())
                .OrderByDescending(fr => fr.SendTime)
                .Select(fr => new ReceivedFriendRequestDto()
                {
                    Id = fr.Id,
                    SendTime = fr.SendTime,
                    Status = (int)fr.Status.ToEnum<FriendRequestStatus>(),
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

            return !await _context.FriendRequests.AnyAsync(fr => fr.Status != FriendRequestStatus.Accepted.ToString()
                && listId.Contains(fr.ReceiverId) && listId.Contains(fr.SenderId))
                && !await _context.Relationships.AnyAsync(r => listId.Contains(r.User1Id) && listId.Contains(r.User2Id));
        }

        public async Task UpdateStatusOfListFriendRequests(string userId)
        {
            var friendRequests = await _context.FriendRequests
            .Where(fr => (fr.SenderId == userId && fr.Status == FriendRequestStatus.Accepted.ToString())
                || (fr.ReceiverId == userId && fr.Status == FriendRequestStatus.NotRead.ToString()))
            .ToListAsync();

            foreach (FriendRequest friendRequest in friendRequests)
            {
                switch (friendRequest.Status)
                {
                    case nameof(FriendRequestStatus.NotRead):
                        friendRequest.Status = FriendRequestStatus.Pending.ToString();
                        break;
                    case nameof(FriendRequestStatus.Accepted):
                        _context.FriendRequests.Remove(friendRequest);
                        break;
                }
            }
        }

        public async Task CreateFriendRequest(string senderId, string receiverId)
        {
            await _context.FriendRequests.AddAsync(new FriendRequest()
            {
                SenderId = senderId,
                ReceiverId = receiverId,
                SendTime = DateTime.Now,
                Status = FriendRequestStatus.NotRead.ToString()
            });
        }

        public async Task<ReceivedFriendRequestDto> GetLastFriendRequestByUser(string senderId, string receiverId)
        {
            return await _context.FriendRequests
                .Include(fr => fr.Sender)
                .ThenInclude(sd => sd.Class)
                .Where(fr => fr.SenderId == senderId && fr.ReceiverId == receiverId)
                .Select(fr => new ReceivedFriendRequestDto()
                {
                    Id = fr.Id,
                    SendTime = fr.SendTime,
                    SenderId = fr.SenderId,
                    SenderName = fr.Sender.Name,
                    SenderAvatar = fr.Sender.Avatar,
                    Status = (int)fr.Status.ToEnum<FriendRequestStatus>(),
                    SenderClassName = fr.Sender.Class.Name
                }).OrderBy(fr => fr.SendTime).LastOrDefaultAsync();
        }
    }
}