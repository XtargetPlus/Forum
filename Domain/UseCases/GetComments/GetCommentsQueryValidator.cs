using FluentValidation;
using Forum.Domain.Exceptions;

namespace Forum.Domain.UseCases.GetComments;

internal class GetCommentsQueryValidator : AbstractValidator<GetCommentsQuery>
{
    public GetCommentsQueryValidator()
    {
        RuleFor(q => q.TopicId).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(q => q.Skip).GreaterThanOrEqualTo(0).WithErrorCode(ValidationErrorCode.Invalid);
        RuleFor(q => q.Take).GreaterThanOrEqualTo(0).WithErrorCode(ValidationErrorCode.Invalid);
    }
}