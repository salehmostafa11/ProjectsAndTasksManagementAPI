
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Projects_And_Tasks_Management_WebAPI.Exceptions;

public class GlobalexceptionHandeler(IProblemDetailsService problemDetailsService) 
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
            BussinessRuleException br=> (br.StatusCode, br.Message),
            _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
        };

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = null,
                Title = "Error has occurred",
                Detail = detail
            }
        });
    }
}