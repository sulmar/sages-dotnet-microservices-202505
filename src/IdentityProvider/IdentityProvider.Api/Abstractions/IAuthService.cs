namespace IdentityProvider.Api.Abstractions;

public interface IAuthService
{
    Task<AuthenticationResult> AuthorizeAsync(string username, string password);
}

public interface ITokenService
{
    string GenerateAccessToken(UserIdentity user);
}


public record AuthenticationResult(bool IsAuthentication, UserIdentity Identity);

public class UserIdentity
{
    public int? Id { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public string? PhoneNumber { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime Birthdate { get; set; }
}