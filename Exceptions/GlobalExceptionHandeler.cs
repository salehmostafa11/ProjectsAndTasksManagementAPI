using System.ComponentModel.DataAnnotations;
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
        httpContext.Response.StatusCode = exception switch
        {
            ValidationException => StatusCodes.Status400BadRequest,
            BussinessRuleException br=> br.StatusCode,
            _ => StatusCodes.Status500InternalServerError
        };

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Type = exception.GetType().Name,
                Title = "Error has occured",
                Detail = exception.Message
            }
        });
    }
}