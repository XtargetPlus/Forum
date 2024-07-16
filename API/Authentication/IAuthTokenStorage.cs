namespace Forum.API.Authentication;

public interface IAuthTokenStorage
{
    (bool success, string token) TryExtract(HttpContext httpContext);
    void Store(HttpContext httpContext, string token);
}