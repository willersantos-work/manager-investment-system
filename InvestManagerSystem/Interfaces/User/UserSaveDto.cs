using InvestManagerSystem.Enums;
using System.Text.Json.Serialization;

namespace InvestManagerSystem.Interfaces.User
{
    public class UserSaveDto
    {
        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string HashPassword { get; set; } = null!;
        
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public UserTypeEnum Type { get; set; }
    }
}
