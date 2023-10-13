using BKConnectBE.Model.Dtos.RoomManagement;
using BKConnectBE.Repository.Rooms;

namespace BKConnectBE.Service.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;

        public RoomService(
            IRoomRepository roomRepository
        )
        {
            _roomRepository = roomRepository;
        }

        public async Task<List<RoomSidebarDto>> GetListOfRoomsByUserId(string userId, string searchKey = "")
        {
            return await _roomRepository.GetListOfRoomsByUserId(userId, searchKey);
        }
    }
}