using System.Text.Json;
using ProductApi.Exceptions;

namespace ProductApi.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }

    private static async Task HandleException(
        HttpContext context,
        Exception exception)
    {
        int statusCode = 500;

        switch (exception)
        {
            case NotFoundException:
                statusCode = 404;
                break;
        }

        var response = new
        {
            statusCode,
            message = exception.Message,
            timestamp = DateTime.UtcNow
        };

        context.Response.ContentType = "application/json";
        context.Response.StatusCode = statusCode;

        var json = JsonSerializer.Serialize(response);

        await context.Response.WriteAsync(json);
    }
}