using API.Authentication;
using Domain.Authentication;

namespace API.Middlewares; 

public class AuthenticationMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext,
        IAuthTokenStorage tokenStorage,
        IAuthenticationService authenticationService,
        IIdentityProvider identityProvider)
    {
        var (success, token) = tokenStorage.TryExtract(httpContext);
        var identity = success
            ? await authenticationService.Authenticate(token, httpContext.RequestAborted)
            : Identity.Guest;
        identityProvider.Current = identity;

        await next(httpContext);
    } 
}
