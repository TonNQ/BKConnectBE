using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Messages
{
    public interface IMessageRepository
    {
        Task<List<Message>> GetAllMessagesInRoomAsync(long roomId);
        Task<Message> GetLastMessageInRoomAsync(long roomId);
        Task<List<Message>> GetAllNoneTextMessagesInRoomAsync(long roomId, string messageType, string userId);
        Task<Message> GetMessageByIdAsync(long messageId);
        Task<string> GetRootMessageSenderIdAsync(long? messageId);
    }
}