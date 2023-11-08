using BKConnectBE.Model.Dtos.RoomManagement;

namespace BKConnectBE.Repository.Rooms
{
    public interface IRoomRepository
    {
        Task<List<RoomSidebarDto>> GetListOfRoomsByUserId(string userId, string searchKey);
        Task<List<string>> GetListOfUserIdInRoomAsync(long roomId);
        Task CreateNewPrivateRoom(string userId1, string userId2);
    }
}