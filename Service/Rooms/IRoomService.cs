using BKConnectBE.Model.Dtos.RoomManagement;

namespace BKConnectBE.Service.Rooms
{
    public interface IRoomService
    {
        Task<List<RoomSidebarDto>> GetListOfRoomsByUserId(string userId, string searchKey = "");
        Task<List<GroupRoomDto>> GetListOfRoomsByTypeAndUserId(string type, string userId);
        Task<List<GroupRoomDto>> SearchListOfRoomsByTypeAndUserId(string type, string userId, string searchKey = "");
        Task<List<string>> GetListOfUserIdInRoomAsync(long roomId);
        Task<RoomDetailDto> GetInformationOfRoom(long roomId, string userId);
        Task<List<MemberOfRoomDto>> GetListOfMembersInRoomAsync(long roomId, string userId);
    }
}