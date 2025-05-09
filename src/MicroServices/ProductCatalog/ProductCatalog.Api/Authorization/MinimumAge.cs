using Microsoft.AspNetCore.Authorization;

namespace ProductCatalog.Api.Authorization;

public record MinimumAgeRequirement(int Age) : IAuthorizationRequirement; // mark interface

