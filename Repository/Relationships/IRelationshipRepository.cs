using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Relationships
{
    public interface IRelationshipRepository
    {
        Task<List<User>> GetListOfFriendsByUserId(string userId);
        Task<List<User>> SearchListOfFriendsByUserId(string userId, string searchKey);
        Task CreateNewRelationship(string userId1, string userId2);
        Task BlockUserAsync(string userId1, string userId2);
        Task UnfriendAsync(string userId1, string userId2);
    }
}