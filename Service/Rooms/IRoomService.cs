using BKConnectBE.Model.Dtos.RoomManagement;

namespace BKConnectBE.Service.Rooms
{
    public interface IRoomService
    {
        Task<List<RoomSidebarDto>> GetListOfRoomsByUserId(string userId, string searchKey = "");
    }
}