using System.Diagnostics.Eventing.Reader;
using BKConnectBE.Model.Dtos.MessageManagement;
using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Model.Dtos.RoomManagement;

namespace BKConnectBE.Service.Rooms
{
    public interface IRoomService
    {
        Task<List<RoomDetailDto>> GetListOfRoomsByUserId(string userId, SearchKeyCondition condition = null);
        Task<List<GroupRoomDto>> GetListOfRoomsByTypeAndUserId(string type, string userId);
        Task<List<GroupRoomDto>> SearchListOfRoomsByTypeAndUserId(string type, string userId, string searchKey = "");
        Task<List<string>> GetListOfUserIdInRoomAsync(long roomId);
        Task<List<MemberOfRoomDto>> GetListOfMembersInRoomAsync(long roomId, string userId);
        Task<SendMessageDto> AddUserToRoomAsync(long roomId, string addUserId, string userId);
        Task<SendMessageDto> RemoveUserFromRoom(long roomId, string removeId, string userId);
        Task<SendMessageDto> LeaveRoom(long roomId, string userId);
        Task<SendMessageDto> CreateGroupRoomAsync(AddGroupRoomDto addGroupRoomDto, string userId);
        Task<ChangedRoomDto> GetChangedRoomInfo(long roomId, string affectedId, bool isAdd = true);
    }
}