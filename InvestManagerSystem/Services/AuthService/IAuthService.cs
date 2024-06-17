using InvestManagerSystem.Interfaces.Auth;

namespace InvestManagerSystem.Services.AuthService
{
    public interface IAuthService
    {
        CredentialResponseDto LoginAdmin(CredentialDto credential);
        CredentialResponseDto LoginClient(CredentialDto credential);
    }
}
