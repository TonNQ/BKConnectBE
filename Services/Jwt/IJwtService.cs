namespace BKConnect.Service.Jwt
{
    public interface IJwtService
    {
        string GenerateAccessToken(string userId, string userName, long roleId);

        Dictionary<string, string> DecodeToken(string token);
    }
}
