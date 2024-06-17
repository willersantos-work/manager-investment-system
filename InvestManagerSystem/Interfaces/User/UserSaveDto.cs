using InvestManagerSystem.Enums;

namespace InvestManagerSystem.Interfaces.User
{
    public class UserSaveDto
    {
        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string HashPassword { get; set; } = null!;
        
        public UserTypeEnum Type { get; set; }
    }
}
