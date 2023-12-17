using BKConnectBE.Model.Dtos.RoomManagement;
using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Rooms
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetListOfRoomsByUserId(string userId);
        Task<List<Room>> GetListOfRoomsByTypeAndUserId(string type, string userId);
        Task<List<string>> GetListOfUserIdInRoomAsync(long roomId);
        Task<List<string>> GetListOfOldUserIdInRoomAsync(long roomId, List<string> newUserId);
        Task<UserOfRoom> GetUserOfRoomInfo(long roomId, string userId);
        Task<User> GetFriendInRoom(long roomId, string userId);
        Task<Room> GetInformationOfRoom(long roomId);
        Task<int?> GetTotalMemberOfRoom(long roomId);
        Task CreateNewPrivateRoom(string userId1, string userId2, string serverMessage);
        Task<List<UserOfRoom>> GetListOfMembersInRoomAsync(long roomId, string userId);
        Task<bool> IsInRoomAsync(long roomId, string userId);
        Task<bool> IsExistBefore(long roomId, string userId);
        Task<bool> IsAdmin(long roomId, string userId);
        Task RemoveUserById(long roomId, string userId);
        Task UpdateAvatar(long roomId, string img);
        Task UpdateName(long roomId, string name);
        Task SetReadMessageOfRoom(string userId, ReadMessageOfRoomDto readMessage);
    }
}