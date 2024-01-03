using BKConnectBE.Model.Dtos.RoomManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Rooms;

namespace BKConnectBE.Service.BackgroundJobs
{
    public class BackgroundJobService : IBackgroundJobService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IGenericRepository<Message> _messageRepository;

        public BackgroundJobService(IRoomRepository roomRepository,
            IGenericRepository<Message> messageRepository)
        {
            _roomRepository = roomRepository;
            _messageRepository = messageRepository;
        }

        public async Task SetReadMessageOfRoom(string userId, long readMessageId)
        {
            await _roomRepository.SetReadMessageOfRoom(userId, readMessageId);
            await _messageRepository.SaveChangeAsync();
        }
    }
}