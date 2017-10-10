using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace AuthorizationLab.AuthorizationHandlers
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            MinimumAgeRequirement requirement)
        {
            if (context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth))
            {
                var dateOfBirth = Convert.ToDateTime(
                    context.User.FindFirst(c => c.Type == ClaimTypes.DateOfBirth).Value);

                int calculatedAge = DateTime.Today.Year - dateOfBirth.Year;

                if (dateOfBirth > DateTime.Today.AddYears(-calculatedAge))
                    calculatedAge--;

                if (calculatedAge >= requirement._minAge)
                    context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
