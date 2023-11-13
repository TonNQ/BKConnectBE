using BKConnect.BKConnectBE.Common;
using BKConnectBE.Model;
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
                    CreatedDate = DateTime.Now
                });
            }
        }

        public async Task CreateNewRelationship(string userId1, string userId2)
        {
            await _context.Relationships.AddAsync(new Relationship()
            {
                User1Id = userId1,
                User2Id = userId2,
                CreatedDate = DateTime.Now
            });
        }

        public async Task<List<User>> GetListOfFriendsByUserId(string userId)
        {
            return await _context.Relationships.Include(f => f.User1).Include(f => f.User2).ThenInclude(u => u.Class)
                .Where(f => f.BlockBy == null && (f.User1Id == userId || f.User2Id == userId))
                .Select(f => f.User1Id == userId ? f.User2 : f.User1).ToListAsync();
        }

        public async Task<List<User>> SearchListOfFriendsByUserId(string userId, string searchKey)
        {
            return await _context.Relationships.Include(f => f.User1).Include(f => f.User2).ThenInclude(u => u.Class)
                .Where(f => f.BlockBy == null && ((f.User1Id == userId && (f.User2Id.Contains(searchKey) || f.User2.Name.Contains(searchKey)))
                    || f.User2Id == userId && (f.User1Id.Contains(searchKey) || f.User1.Name.Contains(searchKey))))
                .Select(f => f.User1Id == userId ? f.User2 : f.User1).ToListAsync();
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