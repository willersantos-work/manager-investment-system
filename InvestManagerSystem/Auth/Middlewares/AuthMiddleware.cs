using InvestManagerSystem.Auth.Decorators;
using InvestManagerSystem.Enums;
using InvestManagerSystem.Global.Configs;
using InvestManagerSystem.Global.Helpers.CustomException;
using InvestManagerSystem.Interfaces.User;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Text;
using System.Text.Json;

namespace InvestManagerSystem.Auth.Middlewares
{
    public class AuthMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AuthMiddleware> _logger;
        private readonly TokenConfig _tokenConfig;

        public AuthMiddleware(RequestDelegate next, ILogger<AuthMiddleware> logger, IOptions<TokenConfig> tokenConfig)
        {
            _next = next;
            _logger = logger;
            _tokenConfig = tokenConfig.Value;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var endpoint = context.GetEndpoint();

            bool isAuthorized = endpoint?.Metadata.GetMetadata<AuthorizeAttribute>() is not null;

            if (isAuthorized)
            {
                await _next(context);
                return;
            }

            if (!context.Request.Headers.TryGetValue("Authorization", out var token))
            {
                _logger.LogWarning("Undefined token.");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    error = new
                    {
                        message = "Invalid token.",
                        statusCode = HttpStatusCode.Unauthorized
                    }
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                return;
            }

            try
            {
                var tokenValues = token.ToString().Split(" ");
                if (tokenValues.Length != 2 || tokenValues[0] != "Bearer")
                {
                    _logger.LogWarning("Invalid token.");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        error = new
                        {
                            message = "Invalid token.",
                            statusCode = HttpStatusCode.Unauthorized
                        }
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                    return;
                }

                token = tokenValues?[1];

                var jwtSecret = _tokenConfig.SecretToken;
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(jwtSecret);

                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var expirationDateClaim = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value;

                if (expirationDateClaim == null || DateTimeOffset.FromUnixTimeSeconds(long.Parse(expirationDateClaim)) < DateTimeOffset.UtcNow)
                {
                    _logger.LogWarning("Expired token.");
                    context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                    context.Response.ContentType = "application/json";

                    var errorResponse = new
                    {
                        error = new
                        {
                            message = "Expired token.",
                            statusCode = HttpStatusCode.Unauthorized
                        }
                    };

                    await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                    return;
                }

                var user = new UserSaveResponseDto
                {
                    Email = jwtToken.Claims.FirstOrDefault(x => x.Type == "Email")?.Value ?? "",
                    FullName = jwtToken.Claims.FirstOrDefault(x => x.Type == "FullName")?.Value ?? "",
                    Id = int.TryParse(jwtToken.Claims.FirstOrDefault(x => x.Type == "Id")?.Value, out var id) ? id : 0,
                    Type = Enum.TryParse<UserTypeEnum>(jwtToken.Claims.FirstOrDefault(x => x.Type == "Type")?.Value, out var userType) ? userType : UserTypeEnum.Client
                };

                context.Items["User"] = user;

                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Token validation failed.");
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                context.Response.ContentType = "application/json";

                var errorResponse = new
                {
                    error = new
                    {
                        message = "Invalid token.",
                        statusCode = HttpStatusCode.Unauthorized
                    }
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(errorResponse));
                return;
            }
        }
    }
}
