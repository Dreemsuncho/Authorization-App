using AuthorizationLab.Resources;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AuthorizationLab.AuthorizationHandlers
{
    public class EditDocumentHandler : AuthorizationHandler<EditDocumentRequirement, Document>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            EditDocumentRequirement requirement,
            Document resource)
        {
            if (resource.Author == context.User.FindFirst(ClaimTypes.Name).Value)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
