using API.Authentication;
using API.Dtos.Requests;
using Domain.UseCases.SignIn;
using Domain.UseCases.SignOn;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[Route("api/accounts")]
[ApiController]
public class AccountController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> SignOn(
        SignOnRequest request,
        CancellationToken cancellationToken)
    {
        var identity = await mediator.Send(new SignOnCommand(request.Login, request.Password), cancellationToken);
        return Ok(identity);
    }

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn(
        SignInRequest request,
        IAuthTokenStorage storage,
        CancellationToken cancellationToken)
    {
        var (identity, token) = await mediator.Send(new SignInCommand(request.Login, request.Password), cancellationToken);
        storage.Store(HttpContext, token);

        return Ok(identity);
    }
}