using BKConnectBE.Model.Dtos.UserManagement;

namespace BKConnectBE.Repository.Relationships
{
    public interface IRelationshipRepository
    {
        Task<List<FriendDto>> GetListOfFriendsByUserId(string userId);
        Task CreateNewRelationship(string userId1, string userId2);
        Task BlockUserAsync(string userId1, string userId2);
        Task UnfriendAsync(string userId1, string userId2);
    }
}