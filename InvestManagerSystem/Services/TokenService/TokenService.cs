using InvestManagerSystem.Global.Configs;
using InvestManagerSystem.Global.Injection;
using InvestManagerSystem.Interfaces.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json;

namespace InvestManagerSystem.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly ILogger<TokenService> _logger;
        private readonly TokenConfig _tokenConfig;

        public TokenService(ILogger<TokenService> logger, IOptions<TokenConfig> tokenConfig)
        {
            _logger = logger;
            _tokenConfig = tokenConfig.Value;
        }

        public async Task<string> GenerateToken(UserSaveResponseDto user)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(GenerateToken)} - Request - {JsonSerializer.Serialize(user)}");
                var tokenHandler = new JwtSecurityTokenHandler();
                var token = await Task.Run(() =>
                {
                    var secret = Encoding.ASCII.GetBytes(_tokenConfig.SecretToken);
                    var expirationDate = DateTime.UtcNow.AddDays(_tokenConfig.TokenValidityDays);

                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new System.Security.Claims.ClaimsIdentity(new[]
                        {
                            new System.Security.Claims.Claim("Email", user.Email),
                            new System.Security.Claims.Claim("FullName", user.FullName),
                            new System.Security.Claims.Claim("Id", user.Id.ToString()),
                            new System.Security.Claims.Claim("Type", user.Type.ToString()),
                        }),
                        Expires = expirationDate,
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
                    };
                    return tokenHandler.CreateToken(tokenDescriptor);
                });

                string accessToken = tokenHandler.WriteToken(token);
                _logger.LogInformation($"end service {nameof(GenerateToken)} - Response - {accessToken}");
                return accessToken;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(GenerateToken)} - Error - {JsonSerializer.Serialize(user)}");
                throw;
            }
        }
    }
}