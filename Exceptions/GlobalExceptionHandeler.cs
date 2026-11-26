
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Projects_And_Tasks_Management_WebAPI.Exceptions;

public class GlobalExceptionHandeler(IProblemDetailsService problemDetailsService,
                                        ILogger<GlobalExceptionHandeler> logger) 
            : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync( 
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {

        var (statusCode, detail) = exception switch
        {
            ValidationException => (StatusCodes.Status400BadRequest, exception.Message),
            BusinessRuleException br=> (br.StatusCode, br.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        if((statusCode == StatusCodes.Status500InternalServerError) && exception is not OperationCanceledException)
            logger.LogError(exception, "Unhandled exception while processing {Path}", httpContext.Request.Path);
        

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = null,
                Detail = detail,
                Status = statusCode
            }
        });
    }
}