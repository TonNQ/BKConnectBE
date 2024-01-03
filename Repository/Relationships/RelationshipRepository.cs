using BKConnect.BKConnectBE.Common;
using BKConnectBE.Model;
using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKConnectBE.Repository.Relationships
{
    public class RelationshipRepository : IRelationshipRepository
    {
        private readonly BKConnectContext _context;

        public RelationshipRepository(BKConnectContext context)
        {
            _context = context;
        }

        public async Task BlockUserAsync(string userId1, string userId2)
        {
            var relationship = await _context.Relationships.FirstOrDefaultAsync(r =>
                (r.User1Id == userId1 && r.User2Id == userId2) || (r.User1Id == userId2 && r.User2Id == userId1));
            if (relationship is not null)
            {
                relationship.BlockBy = userId1;
            }
            else
            {
                await _context.Relationships.AddAsync(new Relationship()
                {
                    User1Id = userId1,
                    User2Id = userId2,
                    BlockBy = userId1,
                    CreatedDate = DateTime.UtcNow.AddHours(7)
                });
            }
        }

        public async Task CreateNewRelationship(string userId1, string userId2)
        {
            await _context.Relationships.AddAsync(new Relationship()
            {
                User1Id = userId1,
                User2Id = userId2,
                CreatedDate = DateTime.UtcNow.AddHours(7)
            });
        }

        public async Task<List<FriendDto>> GetListOfFriendsByUserId(string userId)
        {
            return await _context.Relationships
                .Where(f => f.BlockBy == null && (f.User1Id == userId || f.User2Id == userId))
                .OrderByDescending(f => f.CreatedDate)
                .Select(f => f.User1Id == userId
                    ? new FriendDto()
                    {
                        Id = f.User2Id,
                        Name = f.User2.Name,
                        Avatar = f.User2.Avatar,
                        ClassName = f.User2.Class != null ? f.User2.Class.Name : null,
                        FriendTime = f.CreatedDate,
                    }
                    : new FriendDto()
                    {
                        Id = f.User1Id,
                        Name = f.User1.Name,
                        Avatar = f.User1.Avatar,
                        ClassName = f.User1.Class != null ? f.User1.Class.Name : null,
                        FriendTime = f.CreatedDate,
                    }).ToListAsync();
        }

        public async Task UnfriendAsync(string userId1, string userId2)
        {
            var relationship = await _context.Relationships.FirstOrDefaultAsync(r =>
                (r.User1Id == userId1 && r.User2Id == userId2) || (r.User1Id == userId2 && r.User2Id == userId1))
                ?? throw new Exception(MsgNo.ERROR_RELATIONSHIP_NOT_FOUND);
            _context.Relationships.Remove(relationship);
        }
    }
}