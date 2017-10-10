using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuthorizationLab.AuthorizationHandlers
{
    public class HasBadgeHandler : AuthorizationHandler<OfficeEntryRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OfficeEntryRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == "BadgeNumber" && c.Issuer == "https://contoso.com"))
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
