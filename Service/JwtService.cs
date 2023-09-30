using BKConnectBE;

namespace BKConnect.Service;

public class JwtService : IJwtService
{
    private readonly JwtConfig config;

    public JwtService(IConfiguration configuration) 
    {
        config = configuration.GetSection("Settings").Get<Settings>().jwtConfig;
    }

    public string GenerateAccessToken()
    {
        return config.AccessTokenKey;
    }
}
