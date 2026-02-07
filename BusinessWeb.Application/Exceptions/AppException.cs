namespace BusinessWeb.Application.Exceptions;

public class AppException : Exception
{
    public AppException(string message, int statusCode = 400, string? type = null) : base(message)
    {
        StatusCode = statusCode;
        Type = type ?? "business_error";
    }

    public int StatusCode { get; }
    public string Type { get; }
}
