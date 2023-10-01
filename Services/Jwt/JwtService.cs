using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BKConnectBE;
using Microsoft.IdentityModel.Tokens;

namespace BKConnect.Service.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _config;

        public JwtService(IConfiguration configuration)
        {
            _config = configuration.GetSection("Settings").Get<Settings>().JwtConfig;
        }

        public string GenerateAccessToken(string userId, string userName, long roleId)
        {
            var claims = new List<Claim>()
            {
                new ("Username", userName),
                new ("UserId", userId),
                new ("RoleId", roleId.ToString())
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.AccessTokenKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddDays(_config.AccessTokenExpireDays),
                signingCredentials: credentials
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public Dictionary<string, string> DecodeToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(token);
            return jwt.Claims.ToDictionary(x => x.Type, x => x.Value);
        }
    }

}
