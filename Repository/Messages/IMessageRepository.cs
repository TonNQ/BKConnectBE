using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Messages
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllMessagesInRoomAsync(long roomId);
        Task<Message> GetMessageByIdAsync(long messageId);
    }
}