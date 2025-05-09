using Microsoft.AspNetCore.Authorization;

namespace ProductCatalog.Api.Authorization;

public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
    {
        var type = "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/dateofbirth";

        if (context.User.HasClaim(c => c.Type == type))
        {
            var birthdate = DateTime.Parse(context.User.FindFirst(c => c.Type == type).Value);
            var age = DateTime.Today.Year - birthdate.Year;

            if (age >= requirement.Age)
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }

        return Task.CompletedTask;
    }
}

