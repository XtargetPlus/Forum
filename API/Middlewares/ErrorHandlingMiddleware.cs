using FluentValidation;
using Forum.Domain.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace API.Middlewares;

public class ErrorHandlingMiddleware(RequestDelegate next)
{
    public async Task InvokeAsync(
        HttpContext httpContext,
        ILogger<ErrorHandlingMiddleware> logger,
        ProblemDetailsFactory problemDetailsFactory)
    {
        try
        {
            await next.Invoke(httpContext);    
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex, 
                "Error has happened with {RequestPath}, the message is {ErrorMessage}", 
                httpContext.Request.Path.Value, ex.Message);

            ProblemDetails problemDetails;
            switch (ex)
            {
                case DomainException domainException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, domainException);
                    logger.LogError(domainException, "Domain exception occurred");
                    break;
                case ValidationException validationException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, validationException);
                    logger.LogInformation(validationException, "Somebody sends invalid request");
                    break;
                case System.ComponentModel.DataAnnotations.ValidationException validationException:
                    problemDetails = problemDetailsFactory.CreateFrom(httpContext, validationException);
                    break;
                default:
                    problemDetails = problemDetailsFactory.CreateProblemDetails(httpContext, StatusCodes.Status500InternalServerError, "Unhandled error! Please contact with us.", detail: ex.Message);
                    logger.LogError(ex, "Unhandled exception occurred");
                    break;
            }

            httpContext.Response.StatusCode = problemDetails.Status ?? StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, problemDetails.GetType());
        }
    }
}