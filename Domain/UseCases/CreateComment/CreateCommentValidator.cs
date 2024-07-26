using FluentValidation;
using Forum.Domain.Exceptions;

namespace Forum.Domain.UseCases.CreateComment;

internal class CreateCommentValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentValidator()
    {
        RuleFor(c => c.TopicId).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(c => c.Text)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
            .MaximumLength(255).WithErrorCode(ValidationErrorCode.TooLong);
    }
}