using Microsoft.AspNetCore.Authorization;

namespace ProductCatalog.Api.Authorization;

// Wymaganie oparte o zasoby
public record ProductOwnerRequirement() : IAuthorizationRequirement;

