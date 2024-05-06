using FluentValidation;
using Forum.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace API.Middlewares;

public static class ProblemDetailsFactoryExtensions
{
    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory, 
        HttpContext httpContext,
        DomainException domainException) => 
        factory.CreateProblemDetails(httpContext, 
            domainException.ErrorCode switch
            {
                DomainErrorCode.Gone410 => StatusCodes.Status410Gone,
                DomainErrorCode.Forbidden403 => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError
            },
            domainException.Message);

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory,
        HttpContext httpContext,
        ValidationException validationException)
    {
        var modelStateDictionary = new ModelStateDictionary();
        foreach (var error in validationException.Errors)
        {
            modelStateDictionary.AddModelError(error.PropertyName, error.ErrorCode);
        }

        return factory.CreateValidationProblemDetails(httpContext, 
            modelStateDictionary, 
            StatusCodes.Status400BadRequest, 
            "Validation error", 
            detail: validationException.Message);
    }

    public static ProblemDetails CreateFrom(this ProblemDetailsFactory factory,
        HttpContext httpContext,
        System.ComponentModel.DataAnnotations.ValidationException validationException) =>
        factory.CreateProblemDetails(httpContext,
            StatusCodes.Status400BadRequest,
            "Validation error",
            detail: validationException.Message);
}
