using BKConnectBE.Common;
using BKConnectBE.Common.Enumeration;
using BKConnectBE.Model;
using BKConnectBE.Model.Dtos.Parameters;
using BKConnectBE.Model.Dtos.UserManagement;
using BKConnectBE.Model.Entities;
using Microsoft.EntityFrameworkCore;

namespace BKConnectBE.Repository.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly BKConnectContext _context;

        public UserRepository(BKConnectContext context)
        {
            _context = context;
        }

        public async Task ChangePasswordAsync(string userId, string password)
        {
            User user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            user.Password = password;
        }

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.Include(u => u.Class).ThenInclude(f => f.Faculty).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<List<UserSearchDto>> SearchListOfUsers(string userId, SearchKeyConditionWithPage searchCondition)
        {
            var searchKey = Helper.RemoveUnicodeSymbol(searchCondition.SearchKey);
            var users = await _context.Users.Include(u => u.Class).ToListAsync();
            var relationships = await _context.Relationships.ToListAsync();
            return users.Where(u => u.Id != userId && (u.Id.Contains(searchKey)
                || Helper.RemoveUnicodeSymbol(u.Name).Contains(searchKey)))
                .OrderBy(u => u.Id).Skip((searchCondition.PageIndex - 1) * Constants.DEFAULT_PAGE_SIZE)
                .Take(Constants.DEFAULT_PAGE_SIZE).Select(u => new UserSearchDto()
                {
                    Id = u.Id,
                    Name = u.Name,
                    Avatar = u.Avatar,
                    ClassName = u.Class?.Name,
                    IsFriend = relationships.Any(r => r.BlockBy == null
                        && (r.User1Id == u.Id && r.User2Id == userId)
                        || (r.User2Id == u.Id && r.User1Id == userId))
                }).ToList();
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.Include(u => u.Class).ThenInclude(f => f.Faculty).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            User updatedUser = await _context.Users.Include(u => u.Class).ThenInclude(f => f.Faculty).FirstOrDefaultAsync(u => u.Id == user.Id);
            updatedUser.Name = user.Name;
            updatedUser.DateOfBirth = user.DateOfBirth;
            updatedUser.Gender = user.Gender;
            updatedUser.Avatar = user.Avatar;
            updatedUser.ClassId = (long)user.ClassId;
            if (updatedUser.Role == Role.Teacher.ToString())
            {
                updatedUser.FacultyId = user.FacultyId;
            }
            return updatedUser;
        }
    }
}