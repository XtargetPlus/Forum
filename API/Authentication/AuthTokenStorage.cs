namespace API.Authentication;

internal class AuthTokenStorage : IAuthTokenStorage
{
    private const string HeaderKey = "Forum-Auth-Token";

    public (bool success, string token) TryExtract(HttpContext httpContext)
    {
        if (httpContext.Request.Cookies.TryGetValue(HeaderKey, out var value) && !string.IsNullOrWhiteSpace(value))
        {
            return (true, value);
        }

        return (false, string.Empty);
    }

    public void Store(HttpContext httpContext, string token) => httpContext.Response.Cookies.Append(HeaderKey, token, new CookieOptions
    {
        HttpOnly = true
    });
}