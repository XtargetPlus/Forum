using FluentValidation;
using Forum.Domain.Exceptions;

namespace Forum.Domain.UseCases.CreateForum;

internal class CreateForumCommandValidator : AbstractValidator<CreateForumCommand>
{
    public CreateForumCommandValidator()
    {
        RuleFor(c => c.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
            .MaximumLength(50).WithErrorCode(ValidationErrorCode.TooLong);
    }
}