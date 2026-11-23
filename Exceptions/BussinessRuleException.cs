namespace Projects_And_Tasks_Management_WebAPI.Exceptions;

public class BussinessRuleException(string message, int statusCode) : Exception(message)
{
    public int StatusCode { get; } = statusCode;
}