using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using InventoryControl.Models;

namespace InventoryControl
{
    public class AlbumOwnerAuthHandler : AuthorizationHandler<AlbumOwnerRequirement, Album>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            AlbumOwnerRequirement requirement, 
            Album resource)
        {
            string companyName = context.User.FindFirst(u => u.Type == Constants.CompanyClaimType).Value;

            if (resource.Publisher == companyName)
                context.Succeed(requirement);

            return Task.CompletedTask;
        }
    }
}
