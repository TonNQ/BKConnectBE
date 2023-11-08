using BKConnectBE.Model.Dtos.MessageManagement;

namespace BKConnectBE.Service.Messages
{
    public interface IMessageService
    {
        Task<List<ReceiveMessageDto>> GetAllMessagesInRoomAsync(string userId, long roomId);
        Task<ReceiveMessageDto> AddMessageAsync(SendMessageDto messageDto, string userId);
        Task<string> GetRootMessageSenderId(long? messageId);
        Task<ReceiveMessageDto> RenameUser(ReceiveMessageDto receiveMsg, string userId, string rootSenderId);
    }
}