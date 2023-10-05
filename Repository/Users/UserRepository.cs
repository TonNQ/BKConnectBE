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

        public async Task<User> GetByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}