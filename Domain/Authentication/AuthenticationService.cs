using System.Security.Cryptography;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Domain.Authentication;

internal class AuthenticationService(
        ILogger<AuthenticationService> logger, 
        IAuthenticationStorage storage,
        ISymmetricDecryptor decryptor,
        IOptions<AuthenticationConfiguration> options) 
    : IAuthenticationService
{
    private readonly AuthenticationConfiguration _configuration = options.Value;

    public async Task<IIdentity> Authenticate(string authToken, CancellationToken cancellationToken)
    {
        string sessionIdString;
        try
        {
            sessionIdString = await decryptor.Decrypt(authToken, _configuration.Key, cancellationToken);
        }
        catch (CryptographicException exception)
        {
            logger.LogWarning(exception, "Cannon decrypt auth token: {AuthToken}", authToken);
            return Identity.Guest;
        }

        if (!Guid.TryParse(sessionIdString, out var sessionId)) return Identity.Guest;

        var session = await storage.FindSession(sessionId, cancellationToken);
        if (session is null) return Identity.Guest;
        if (session.ExpiresAt < DateTimeOffset.UtcNow) return Identity.Guest;

        return new Identity(session.UserId, sessionId);
    }
}