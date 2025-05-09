using Microsoft.AspNetCore.Authorization;
using ProductCatalog.Domain.Entities;

namespace ProductCatalog.Api.Authorization;

public class ProductOwnerHandler : AuthorizationHandler<ProductOwnerRequirement, Product>
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ProductOwnerRequirement requirement, Product resource)
    {
        if (resource.Owner == context.User.Identity.Name)
        {
            context.Succeed(requirement);
        }
        else
        {
            context.Fail();
        }

        return Task.CompletedTask;
    }
}

