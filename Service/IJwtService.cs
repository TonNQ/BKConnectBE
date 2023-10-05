using System.Security.Claims;
using BKConnectBE.Model.Entities;

namespace BKConnect.Service
{
    public interface IJwtService
    {
        public string GenerateAccessToken(string userId, string username, string role);
        public Task<RefreshToken> GenerateRefreshTokenAsync(string userId, string username, string role);
        public string ValidateToken(bool isAccessToken, string token);
        public IEnumerable<Claim> GetTokenClaims(string tokenString);
        public string GetNewAccessToken(string refreshToken);
    }
}


