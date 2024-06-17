using InvestManagerSystem.Interfaces.User;

namespace InvestManagerSystem.Interfaces.Auth
{
    public class CredentialResponseDto
    {
        public UserSaveResponseDto User { get; set; } = null!;

        public string AccessToken { get; set; } = null!;

        public CredentialResponseDto(UserSaveResponseDto user, string accessToken)
        {
            User = user;
            AccessToken = accessToken;
        }
    }
}
