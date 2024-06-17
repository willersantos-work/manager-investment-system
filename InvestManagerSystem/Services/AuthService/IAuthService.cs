using InvestManagerSystem.Interfaces.Auth;

namespace InvestManagerSystem.Services.AuthService
{
    public interface IAuthService
    {
        Task<CredentialResponseDto> LoginAdmin(CredentialDto credential);
        Task<CredentialResponseDto> LoginClient(CredentialDto credential);
    }
}
