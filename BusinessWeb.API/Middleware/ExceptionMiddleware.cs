using System.Net;
using BusinessWeb.Application.Exceptions;
using FluentValidation;

namespace BusinessWeb.API.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(ILogger<ExceptionMiddleware> logger)
        => _logger = logger;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException vex)
        {
            context.Response.StatusCode = (int)HttpStatusCode.UnprocessableEntity;
            context.Response.ContentType = "application/json";

            var errors = vex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(g => g.Key, g => g.Select(x => x.ErrorMessage).ToArray());

            await context.Response.WriteAsJsonAsync(new
            {
                type = "validation_error",
                title = "Validation failed",
                status = 422,
                errors
            });
        }
        catch (AppException aex)
        {
            context.Response.StatusCode = aex.StatusCode;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                type = aex.Type,
                title = aex.Message,
                status = aex.StatusCode
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled error");

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                type = "server_error",
                title = "Unexpected error",
                status = 500
            });
        }
    }
}
