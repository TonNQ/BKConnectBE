using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.Relationships
{
    public interface IRelationshipRepository
    {
        Task<List<User>> GetListOfFriendsByUserId(string userId);
        Task<List<User>> SearchListOfFriendsByUserId(string userId, string searchKey);
    }
}