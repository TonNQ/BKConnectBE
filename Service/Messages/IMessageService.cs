using BKConnectBE.Model.Dtos.MessageManagement;

namespace BKConnectBE.Service.Messages
{
    public interface IMessageService
    {
        Task<List<ReceiveMessageDto>> GetAllMessagesInRoomAsync(string userId, long roomId);
        Task<ReceiveMessageDto> AddMessageAsync(SendMessageDto messageDto);
    }
}