using BKConnectBE.Model;
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
            return await _context.Users.Include(u => u.Class).FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            return await _context.Users.Include(u => u.Class).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User> UpdateUserAsync(User user)
        {
            User updatedUser = await _context.Users.Include(u => u.Class).FirstOrDefaultAsync(u => u.Id == user.Id);
            updatedUser.Name = user.Name;
            updatedUser.DateOfBirth = user.DateOfBirth;
            updatedUser.Gender = user.Gender;
            updatedUser.Avatar = user.Avatar;
            updatedUser.ClassId = (long)user.ClassId;
            return updatedUser;
        }
    }
}