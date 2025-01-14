using Aplication.Services;
using Core.ConfigurationProp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace Infrastracture.Auth.Authontication
{
    public class PermissionAuthorizationHandler//this class will inject in DI as singleton, so we can't use ctor to get needed values
    : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory serviceScopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            this.serviceScopeFactory = serviceScopeFactory;
        }
        protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(
                c => c.Type == CustomClaims.UserId);

            if (userId is null || !Guid.TryParse(userId.Value, out var id))
            {
                return;
            }
            using var scope = serviceScopeFactory.CreateScope();
            var getUserService = scope.ServiceProvider.GetRequiredService<IUserGetService>();
            var permissions = await getUserService.GetPermissions(id);

            if (permissions.Intersect(requirement.Permissions).Any())
            {
                context.Succeed(requirement);
            }
        }
    }
}
