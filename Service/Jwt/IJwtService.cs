using BKConnectBE.Model.Dtos.RefreshTokenManagement;

namespace BKConnect.Service.Jwt
{
    public interface IJwtService
    {
        string GenerateAccessToken(string userId, string userName, string role);

        Dictionary<string, string> DecodeToken(string token);

        public Task<RefreshTokenDto> GenerateRefreshTokenAsync(string userId, string username, string role);

        public string ValidateToken(bool isAccessToken, string token);

        public string GetNewAccessToken(string refreshToken);
    }
}