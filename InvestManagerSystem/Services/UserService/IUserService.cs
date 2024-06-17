using InvestManagerSystem.Enums;
using InvestManagerSystem.Interfaces.Auth;
using InvestManagerSystem.Interfaces.User;

namespace InvestManagerSystem.Services.UserService
{
    public interface IUserService
    {
        UserSaveResponseDto VerifyCredential(CredentialDto credential, UserTypeEnum userType);
        UserSaveResponseDto GetByEmail(string email);
        IList<UserSaveResponseDto> GetByType(UserTypeEnum type);
        UserSaveResponseDto Register(UserCreateDto newUser, UserTypeEnum type);
    }
}
