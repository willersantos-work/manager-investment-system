using InvestManagerSystem.Enums;

namespace InvestManagerSystem.Interfaces.User
{
    public class UserSaveResponseDto
    {
        public int Id { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public UserTypeEnum Type { get; set; }
    }
}
