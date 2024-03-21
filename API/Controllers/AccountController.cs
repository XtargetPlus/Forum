using API.Authentication;
using API.Dtos.Requests;
using Domain.UseCases.SignIn;
using Domain.UseCases.SignOn;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SignOn(
        SignOnRequest request,
        ISignOnUseCase useCase,
        CancellationToken cancellationToken)
    {
        var identity = await useCase.Execute(new SignOnCommand(request.Login, request.Password), cancellationToken);
        return Ok(identity);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(
        SignInRequest request,
        ISignInUseCase useCase,
        IAuthTokenStorage storage,
        CancellationToken cancellationToken)
    {
        var (identity, token) = await useCase.Execute(new SignInCommand(request.Login, request.Password), cancellationToken);
        storage.Store(HttpContext, token);

        return Ok(identity);
    }
}