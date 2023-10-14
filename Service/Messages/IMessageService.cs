using BKConnectBE.Model.Dtos.MessageManagement;

namespace BKConnectBE.Service.Messages
{
    public interface IMessageService
    {
        Task<List<MessageDto>> GetAllMessagesInRoomAsync(string userId, long roomId);
    }
}