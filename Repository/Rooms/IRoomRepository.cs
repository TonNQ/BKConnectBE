using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Rooms
{
    public interface IRoomRepository
    {
        Task<List<Room>> GetListOfRoomsByUserId(string userId);
        Task<List<string>> GetListOfUserIdInRoomAsync(long roomId);
        Task<Room> GetInformationOfRoom(long roomId);
        Task CreateNewPrivateRoom(string userId1, string userId2, string serverMessage);
        Task<List<UserOfRoom>> GetListOfMembersInRoomAsync(long roomId, string userId);
    }
}