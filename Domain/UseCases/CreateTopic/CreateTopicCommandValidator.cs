﻿using Domain.Dtos;
using Domain.Exceptions;
using FluentValidation;

namespace Domain.UseCases.CreateTopic;

internal class CreateTopicCommandValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicCommandValidator()
    {
        RuleFor(c => c.ForumId).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        RuleFor(c => c.Title)
            .Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
            .MaximumLength(100).WithErrorCode(ValidationErrorCode.TooLong);
    }
}