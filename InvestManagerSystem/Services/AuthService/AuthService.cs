using InvestManagerSystem.Enums;
using InvestManagerSystem.Interfaces.Auth;
using InvestManagerSystem.Interfaces.User;
using InvestManagerSystem.Services.TokenService;
using InvestManagerSystem.Services.UserService;
using System.Text.Json;

namespace InvestManagerSystem.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly ILogger<AuthService> _logger;
        private readonly IUserService _userService;
        private readonly ITokenService _tokenService;

        public AuthService(ILogger<AuthService> logger, IUserService userService, ITokenService tokenService)
        {
            _logger = logger;
            _userService = userService;
            _tokenService = tokenService;
        }

        public async Task<CredentialResponseDto> LoginAdmin(CredentialDto credential)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(LoginAdmin)} - Request - {credential.Email}");
                var response = await Login(credential, UserTypeEnum.Admin);
                _logger.LogInformation($"end service {nameof(LoginAdmin)} - Response - {JsonSerializer.Serialize(response)}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(LoginAdmin)} - Error - {credential.Email}");
                throw;
            }
        }

        public async Task<CredentialResponseDto> LoginClient(CredentialDto credential)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(LoginClient)} - Request - {credential.Email}");
                var response = await Login(credential, UserTypeEnum.Client);
                _logger.LogInformation($"end service {nameof(LoginClient)} - Response - {JsonSerializer.Serialize(response)}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(LoginClient)} - Error - {credential.Email}");
                throw;
            }
        }

        private async Task<CredentialResponseDto> Login(CredentialDto credential, UserTypeEnum userType)
        {
            try
            {
                _logger.LogInformation($"start service {nameof(Login)} - Request - {credential.Email} {userType}");
                UserSaveResponseDto user = _userService.VerifyCredential(credential, userType);
                string accessToken = await _tokenService.GenerateToken(user);

                var response = new CredentialResponseDto(user, accessToken);
                _logger.LogInformation($"end service {nameof(Login)} - Response - {JsonSerializer.Serialize(response)}");
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"error service {nameof(Login)} - Error - {credential.Email} {userType}");
                throw;
            }
        }
    }
}