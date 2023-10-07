using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AutoMapper;
using BKConnect.BKConnectBE.Common;
using BKConnectBE;
using BKConnectBE.Common;
using BKConnectBE.Model.Dtos.RefreshTokenManagement;
using BKConnectBE.Model.Entities;
using BKConnectBE.Repository;
using Microsoft.IdentityModel.Tokens;

namespace BKConnect.Service.Jwt
{
    public class JwtService : IJwtService
    {
        private readonly JwtConfig _config;
        private readonly IGenericRepository<RefreshToken> _genericRepositoryForRefreshToken;
        private readonly IMapper _mapper;
        private readonly AutoMapper.IConfigurationProvider _configurationProvider;

        public JwtService(IConfiguration configuration,
            IGenericRepository<RefreshToken> genericRepositoryForRefreshToken)
        {
            _config = configuration.GetSection("Settings").Get<Settings>().JwtConfig;
            _genericRepositoryForRefreshToken = genericRepositoryForRefreshToken;
            _configurationProvider = new MapperConfiguration(cfg => cfg.AddProfile<MappingProfile>());
            _mapper = new Mapper(_configurationProvider);
        }

        public string GenerateAccessToken(string userId, string userName, string role)
        {
            var claims = new List<Claim>()
            {
                new ("Username", userName),
                new ("UserId", userId),
                new ("Role", role),
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

        public string GenerateTemporaryCode(string userId)
        {
            var claims = new List<Claim>()
            {
                new ("UserId", userId),
                new ("Date", DateTime.Now.ToString("dd-MM-yyyy")),
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

        public async Task<RefreshTokenDto> GenerateRefreshTokenAsync(string userId, string username, string role)
        {
            var claims = new List<Claim>
            {
                new("Username", username),
                new ("UserId", userId),
                new ("Role", role)
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
            await _genericRepositoryForRefreshToken.SaveChangeAsync();
            return _mapper.Map<RefreshTokenDto>(refreshToken); ;
        }

        public string GetNewAccessToken(string refreshToken)
        {
            Dictionary<string, string> tokenInfo = DecodeToken(refreshToken);
            return GenerateAccessToken(tokenInfo["UserId"], tokenInfo["Username"], tokenInfo["Role"]);
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