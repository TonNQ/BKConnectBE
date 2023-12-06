using System.Net.Http.Headers;
using BKConnect.BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model;
using BKConnectBE.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKConnectBE.Repository.Messages
{
    public class MessageRepository : IMessageRepository
    {
        private readonly BKConnectContext _context;
        public MessageRepository(BKConnectContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetAllMessagesInRoomAsync(long roomId)
        {
            return await _context.Rooms.Where(r => r.Id == roomId).SelectMany(r => r.Messages)
                .Include(m => m.Sender).Include(m => m.RootMessage).OrderByDescending(m => m.SendTime).ToListAsync();
        }

        public async Task<List<Message>> GetAllNoneTextMessagesInRoomAsync(long roomId, string messageType, string userId)
        {
            if (!await _context.UsersOfRoom.AnyAsync(ur => ur.RoomId == roomId && ur.UserId == userId))
            {
                throw new Exception(MsgNo.ERROR_UNHADLED_ACTION);
            }
            return await _context.Messages
                .Where(r => r.RoomId == roomId && r.TypeOfMessage == messageType)
                .Include(m => m.RootMessage).OrderByDescending(m => m.SendTime).ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(long messageId)
        {
            return await _context.Messages.Include(m => m.Sender).Include(m => m.RootMessage)
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }

        public async Task<string> GetRootMessageSenderIdAsync(long? messageId)
        {
            Message message = await _context.Messages.FirstOrDefaultAsync(m => m.Id == messageId);
            return message?.SenderId;
        }
    }
}