using IdentityProvider.Api.Abstractions;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;

namespace IdentityProvider.Api.Infrastructures;

public class AuthService : IAuthService
{
    public Task<AuthenticationResult> AuthorizeAsync(string username, string password)
    {
        if (username == "admin" && password == "123")
        {
            var userIdentity = new UserIdentity { Id = 1, FirstName = "John", LastName = "Smith", Username = "admin", Email = "john@example.com", PhoneNumber = "5554567890", Birthdate  = DateTime.Parse("2010-01-01") };

            return Task.FromResult(new AuthenticationResult(true, userIdentity));
        }

        return Task.FromResult(new AuthenticationResult(false, null));
    }
}


public class FakeTokenService : ITokenService
{
    public string GenerateAccessToken(UserIdentity user)
    {
        return "abc";
    }
}

public class JwtTokenService : ITokenService
{
    public string GenerateAccessToken(UserIdentity userIdentity)
    {
        var claims = new Dictionary<string, object>
        {
            [JwtRegisteredClaimNames.Jti] = Guid.NewGuid().ToString(),
            [JwtRegisteredClaimNames.Name] = userIdentity.Username,
            [JwtRegisteredClaimNames.Email] = userIdentity.Email,
            [JwtRegisteredClaimNames.PhoneNumber] = userIdentity.PhoneNumber,
            [JwtRegisteredClaimNames.GivenName] = userIdentity.FirstName,
            [JwtRegisteredClaimNames.FamilyName] = userIdentity.LastName,
            [JwtRegisteredClaimNames.Birthdate] = userIdentity.Birthdate.ToShortDateString(),
            [ClaimTypes.Role] = "Admin",
        };

        string secretKey = "a-string-secret-at-least-256-bits-long";

        var descriptor = new SecurityTokenDescriptor
        {
            Issuer = "https://sages.pl",
            Audience = "https://example.com",
            Expires = DateTime.UtcNow.AddMinutes(30),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)), SecurityAlgorithms.HmacSha256Signature),
            Claims = claims
        };

        var jwt_token = new JsonWebTokenHandler().CreateToken(descriptor);

        return jwt_token;
    }
}
