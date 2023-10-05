using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using BKConnect.BKConnectBE.Common;
using BKConnectBE;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using Microsoft.IdentityModel.Tokens;

namespace BKConnect.Service
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _config;
        private readonly IGenericRepository<RefreshToken> _genericRepositoryForRefreshToken;

        public JwtService(IConfiguration configuration, IGenericRepository<RefreshToken> genericRepositoryForRefreshToken)
        {
            _config = configuration.GetSection("Settings").Get<Settings>().jwtConfig;
            _genericRepositoryForRefreshToken = genericRepositoryForRefreshToken;
        }

        public string GenerateAccessToken(string userId, string userName, string role)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim("Username", userName),
                new Claim("UserId", userId),
                new Claim("Role", role)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.AccessTokenKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(_config.AccessTokenExpireDays),
                signingCredentials: credentials
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public async Task<RefreshToken> GenerateRefreshTokenAsync(string userId, string username, string role)
        {
            List<Claim> claims = new List<Claim>
        {
            new Claim("Username", username),
            new Claim("UserId", userId),
            new Claim("Role", role)
        };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.RefreshTokenKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMonths(_config.RefreshTokenExpireMonths),
                signingCredentials: credentials
            );
            var tokenHandler = new JwtSecurityTokenHandler();

            var refreshToken = new RefreshToken
            {
                UserId = userId,
                Token = tokenHandler.WriteToken(token),
            };
            await _genericRepositoryForRefreshToken.AddAsync(refreshToken);
            await _genericRepositoryForRefreshToken.SaveAsync();
            return refreshToken;
        }

        public string GetNewAccessToken(string refreshToken)
        {
            IEnumerable<Claim> claims = GetTokenClaims(refreshToken);

            Claim userId = claims.FirstOrDefault(c => c.Type == "UserId");
            Claim username = claims.FirstOrDefault(c => c.Type == "Username");
            Claim role = claims.FirstOrDefault(c => c.Type == "Role");

            return GenerateAccessToken(userId.Value, username.Value, role.Value);
        }

        public IEnumerable<Claim> GetTokenClaims(string tokenString)
        {
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(tokenString);
            return token.Claims;
        }

        public string ValidateToken(bool isAccessToken, string token)
        {
            if (token == null)
            {
                throw new Exception(MsgNo.ERROR_TOKEN_INVALID);
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = isAccessToken ? new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.AccessTokenKey))
            : new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config.RefreshTokenKey));
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = key,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var userId = jwtToken.Claims.First(x => x.Type == "UserId").Value.ToString();

                return userId;
            }
            catch
            {
                throw new Exception(MsgNo.ERROR_TOKEN_INVALID);
            }
        }
    }
}


