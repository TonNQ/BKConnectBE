using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Messages
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllMessagesInRoomAsync(long roomId);
        Task<List<Message>> GetAllImageMessagesInRoomAsync(long roomId, string userId);
        Task<Message> GetMessageByIdAsync(long messageId);
        Task<string> GetRootMessageSenderIdAsync(long? messageId);
    }
}