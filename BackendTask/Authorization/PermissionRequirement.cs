using BackendTask.Shared;
using Microsoft.AspNetCore.Authorization;

namespace BackendTask.API.Authorization
{
    internal class PermissionRequirement : IAuthorizationRequirement
    {
        public string Permission { get; }

        public PermissionRequirement(string permission)
        {
            Permission = permission;
        }
    }

    internal class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            PermissionRequirement requirement)
        {
            if (context.User.HasClaim(Constants.PermissionsClaimType, requirement.Permission))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
