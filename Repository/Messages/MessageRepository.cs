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
                .Include(m => m.Sender).Include(m => m.RootMessage).ToListAsync();
        }

        public async Task<Message> GetMessageByIdAsync(long messageId)
        {
            return await _context.Messages.Include(m => m.Sender).Include(m => m.RootMessage)
                .FirstOrDefaultAsync(m => m.Id == messageId);
        }
    }
}