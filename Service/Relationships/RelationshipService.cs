using AutoMapper;
using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using BKConnectBE.Repository.Relationships;

namespace BKConnectBE.Service.Relationships
{
    public class RelationshipService : IRelationshipService
    {
        private readonly IRelationshipRepository _relationshipRepository;
        private readonly IGenericRepository<Relationship> _genericRepositoryForRelationship;
        private readonly IMapper _mapper;

        public RelationshipService(IRelationshipRepository relationshipRepository,
            IGenericRepository<Relationship> genericRepositoryForRelationship,
            IMapper mapper)
        {
            _relationshipRepository = relationshipRepository;
            _genericRepositoryForRelationship = genericRepositoryForRelationship;
            _mapper = mapper;
        }

        public async Task<List<FriendDto>> GetListOfFriendsByUserId(string userId)
        {
            List<User> users = await _relationshipRepository.GetListOfFriendsByUserId(userId);
            return _mapper.Map<List<FriendDto>>(users);
        }

        public async Task<List<FriendDto>> SearchListOfFriendsByUserId(string userId, string searchKey)
        {
            List<User> users = await _relationshipRepository.SearchListOfFriendsByUserId(userId, searchKey);
            return _mapper.Map<List<FriendDto>>(users);
        }

        public async Task UnfriendAsync(string userId1, string userId2)
        {
            await _relationshipRepository.UnfriendAsync(userId1, userId2);
            await _genericRepositoryForRelationship.SaveChangeAsync();
        }
    }
}