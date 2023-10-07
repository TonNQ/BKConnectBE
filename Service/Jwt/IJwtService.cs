using BKConnectBE.Model.Dtos.RefreshTokenManagement;

namespace BKConnect.Service.Jwt
{
    public interface IJwtService
    {
        string GenerateAccessToken(string userId, string userName, string role);

        string GenerateTemporaryCode(string userId);

        Dictionary<string, string> DecodeToken(string token);

        Task<RefreshTokenDto> GenerateRefreshTokenAsync(string userId, string username, string role);

        string ValidateToken(bool isAccessToken, string token);

        string GetNewAccessToken(string refreshToken);
    }
}