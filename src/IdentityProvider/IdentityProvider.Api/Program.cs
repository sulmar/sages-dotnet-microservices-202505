using IdentityProvider.Api.Abstractions;
using IdentityProvider.Api.Infrastructures;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ITokenService, JwtTokenService>();

var app = builder.Build();

app.MapPost("/api/login", async (LoginRequest request, IAuthService authService, ITokenService tokenService) => { 

    var result = await authService.AuthorizeAsync(request.Username, request.Password);

    if (result.IsAuthentication)
    {
        // JWT (Json Web Token)

        var accessToken = tokenService.GenerateAccessToken(result.Identity);

        return Results.Ok(accessToken);
    }

    return Results.Unauthorized();

});

app.Run();

record LoginRequest(string Username, string Password);
