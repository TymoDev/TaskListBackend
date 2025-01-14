using Core.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Api.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
    public class RequirePermissionsAttribute : AuthorizeAttribute
    {
        public RequirePermissionsAttribute(params Permission[] permissions)
        {
            Policy = $"Permissions:{string.Join(",", permissions)}";
        }
    }
}
