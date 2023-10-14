using BKConnectBE.Model.Dtos.UserManagement;

namespace BKConnectBE.Service.Relationships
{
    public interface IRelationshipService
    {
        Task<List<FriendDto>> GetListOfFriendsByUserId(string userId);
        Task<List<FriendDto>> SearchListOfFriendsByUserId(string userId, string searchKey);
    }
}