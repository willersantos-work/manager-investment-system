using InvestManagerSystem.Interfaces.User;

namespace InvestManagerSystem.Services.TokenService
{
    public interface ITokenService
    {
        Task<string> GenerateToken(UserSaveResponseDto user);
    }
}
