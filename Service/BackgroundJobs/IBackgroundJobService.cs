using BKConnectBE.Model.Dtos.RoomManagement;

namespace BKConnectBE.Service.BackgroundJobs
{
    public interface IBackgroundJobService
    {
        Task SetReadMessageOfRoom(string userId, ReadMessageOfRoomDto readMessage);
    }
}