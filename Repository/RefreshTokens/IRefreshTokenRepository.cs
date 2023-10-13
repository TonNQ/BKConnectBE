using BKConnectBE.Model.Entities;

namespace BKConnectBE.Repository.RefreshTokens
{
    public interface IRefreshTokenRepository
    {
        Task<long> GetRefreshTokenId(string refreshToken);

        bool IsValidToken(long refreshTokenId, string userId);
    }
}