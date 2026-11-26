namespace Projects_And_Tasks_Management_WebAPI.Exceptions;

public class BusinessRuleException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}