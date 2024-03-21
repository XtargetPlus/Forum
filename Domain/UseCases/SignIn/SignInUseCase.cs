using Domain.Authentication;
using Domain.Exceptions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Options;

namespace Domain.UseCases.SignIn;

internal class SignInUseCase(
        IOptions<AuthenticationConfiguration> options,
        ISymmetricEncryptor encryptor,
        IPasswordManager passwordManager,
        ISignInStorage storage,
        IValidator<SignInCommand> validator)
    : ISignInUseCase
{
    private readonly AuthenticationConfiguration _configuration = options.Value;

    public async Task<(IIdentity identity, string token)> Execute(SignInCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var recognizedUser = await storage.FindUser(command.Login, cancellationToken);
        if (recognizedUser is null) throw new ValidationException(new ValidationFailure[]
        {
            new()
            {
                PropertyName = nameof(command.Login), 
                ErrorCode = ValidationErrorCode.Invalid,
                AttemptedValue = command.Login
            }
        });

        var passwordMatches = passwordManager.ComparePasswords(command.Password, recognizedUser.Salt, recognizedUser.PasswordHash);
        if (!passwordMatches) throw new ValidationException(new ValidationFailure[]
        {
            new()
            {
                PropertyName = nameof(command.Password),
                ErrorCode = ValidationErrorCode.Invalid,
                AttemptedValue = command.Password
            }
        });

        var sessionId = await storage.CreateSession(recognizedUser.UserId, DateTimeOffset.UtcNow + TimeSpan.FromHours(1), cancellationToken);
        var token = await encryptor.Encrypt(sessionId.ToString(), _configuration.Key, cancellationToken);
        return (new Identity(recognizedUser.UserId, sessionId), token);
    }
}