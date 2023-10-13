using BKConnectBE.Model;
using Microsoft.EntityFrameworkCore;

namespace BKConnectBE.Repository.RefreshTokens
{
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly BKConnectContext _context;

        public RefreshTokenRepository(BKConnectContext context)
        {
            _context = context;
        }

        public async Task<long> GetRefreshTokenId(string refreshToken)
        {
            return (await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.Token == refreshToken))?.Id ?? 0;
        }

        public bool IsValidToken(long refreshTokenId, string userId)
        {
            return refreshTokenId == 0
                || _context.RefreshTokens.Any(r => r.Id == refreshTokenId && r.UserId == userId);
        }
    }
}