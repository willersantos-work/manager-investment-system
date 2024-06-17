using InvestManagerSystem.Enums;
using InvestManagerSystem.Interfaces.User;

namespace InvestManagerSystem.Auth.Context
{
    public static class UserContext
    {
        public static int GetId(this HttpContext context)
        {
            var currentUser = context.GetUser();
            return currentUser.Id;
        }

        public static string GetFullName(this HttpContext context)
        {
            var currentUser = context.GetUser();
            return currentUser.FullName;
        }

        public static string GetEmail(this HttpContext context)
        {
            var currentUser = context.GetUser();
            return currentUser.Email;
        }

        public static UserTypeEnum GetUserType(this HttpContext context)
        {
            var currentUser = context.GetUser();
            return currentUser.Type;
        }

        public static UserSaveResponseDto GetUser(this HttpContext context)
        {
            context.Items.TryGetValue("User", out var userObj);
            return (UserSaveResponseDto) userObj;
        }
    }
}
