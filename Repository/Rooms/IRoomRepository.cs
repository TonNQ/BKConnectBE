using BKConnectBE.Model.Dtos.RoomManagement;
using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Rooms
{
    public interface IRoomRepository
    {
        Task<List<RoomSidebarDto>> GetListOfRoomsByUserId(string userId, string searchKey);
        Task<List<string>> GetListOfUserIdInRoomAsync(long roomId);
        Task<Room> GetInformationOfRoom(long roomId);
        Task CreateNewPrivateRoom(string userId1, string userId2);
        Task<List<UserOfRoom>> GetListOfMembersInRoomAsync(long roomId, string userId);
    }
}