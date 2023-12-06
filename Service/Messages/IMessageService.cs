using BKConnectBE.Model.Dtos.MessageManagement;

namespace BKConnectBE.Service.Messages
{
    public interface IMessageService
    {
        Task<List<ReceiveMessageDto>> GetAllMessagesInRoomAsync(string userId, long roomId);
        Task<List<FileDto>> GetAllNoneTextMessagesInRoomAsync(long roomId, string messageType, string userId);
        Task<ReceiveMessageDto> AddMessageAsync(SendMessageDto messageDto, string userId, string? affectedId = null);
        Task<string> GetRootMessageSenderId(long? messageId);
        Task<ReceiveMessageDto> ChangeMessage(ReceiveMessageDto receiveMsg, string userId, string rootSenderId);
        Task<ReceiveMessageDto> ChangeSystemMessage(ReceiveMessageDto receiveMsg, string userId, string? receiverId, string type);
        Task<string> ChangeContentSystemMessage(long messageId, string userId);
    }
}