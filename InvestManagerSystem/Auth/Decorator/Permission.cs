using InvestManagerSystem.Interfaces.User;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using InvestManagerSystem.Enums;
using InvestManagerSystem.Global.Helpers.CustomException;
using System.Net;

namespace InvestManagerSystem.Auth.Decorator
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class PermissionAttribute : TypeFilterAttribute
    {
        public PermissionAttribute(params UserTypeEnum[] allowedUserTypes) : base(typeof(PermissionFilter))
        {
            Arguments = new object[] { allowedUserTypes };
        }
    }

    public class PermissionFilter : IAuthorizationFilter
    {
        private readonly UserTypeEnum[] _permission;

        public PermissionFilter(UserTypeEnum[] permission)
        {
            _permission = permission;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.Items["User"] as UserSaveResponseDto;

            if (user is null)
            {
                throw new CustomException(HttpStatusCode.Unauthorized, "Invalid token");
            }

            if (!UserHasPermission(user.Type, _permission))
            {
                throw new CustomException(HttpStatusCode.Forbidden, "This user does not have permission enough.");
            }
        }

        private bool UserHasPermission(UserTypeEnum type, UserTypeEnum[] permission)
        {
            return permission.Contains(type);
        }
    }
}
