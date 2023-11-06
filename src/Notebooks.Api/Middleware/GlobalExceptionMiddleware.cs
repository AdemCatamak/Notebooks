using System.Net;
using System.Text;
using Newtonsoft.Json;
using Notebooks.Domain.Exceptions;

namespace Notebooks.Api.Middleware;

public class GlobalExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public GlobalExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext, ILogger<GlobalExceptionHandlerMiddleware> logger)
    {
        try
        {
            httpContext.Request.EnableBuffering();
            await _next.Invoke(httpContext);
        }
        catch (Exception e)
        {
            await HandleAsync(e, logger, httpContext);
        }
    }

    private async Task HandleAsync(Exception exception, ILogger logger, HttpContext context)
    {
        if (exception is not BaseException)
        {
            string requestAsText = await RequestAsTextAsync(context);
            logger.LogError(exception, "{RequestAsText}{NewLine1}{NewLine2}{ExceptionMessage}", requestAsText, Environment.NewLine, Environment.NewLine, exception.Message);
        }

        HttpStatusCode statusCode = DecideStatusCode(exception);
        object payload = DecidePayload(exception);

        string errorHttpContentStr = JsonConvert.SerializeObject(payload);

        context.Response.StatusCode = (int)statusCode;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsync(errorHttpContentStr);
    }

    private static HttpStatusCode DecideStatusCode(Exception exception)
    {
        return exception switch
        {
            NotFoundException => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError
        };
    }

    private static object DecidePayload(Exception exception)
    {
        object payload = exception switch
        {
            BaseException baseException => new ErrorResponse(baseException.Message),
            _ => new { Message = "Unexpected error occurs" }
        };

        return payload;
    }

    private static async Task<string> RequestAsTextAsync(HttpContext httpContext)
    {
        string rawRequestBody = await GetRawBodyAsync(httpContext.Request);

        IEnumerable<string> headerLine = httpContext.Request
            .Headers
            .Where(h => h.Key != "Authentication")
            .Select(pair => $"{pair.Key} => {string.Join("|", pair.Value.ToList())}");
        string headerText = string.Join(Environment.NewLine, headerLine);

        string message =
            $"Request: {httpContext.Request.Scheme}://{httpContext.Request.Host}{httpContext.Request.Path}{httpContext.Request.QueryString}{Environment.NewLine}" +
            $"Headers: {Environment.NewLine}{headerText}{Environment.NewLine}" +
            $"Content : {Environment.NewLine}{rawRequestBody}";

        return message;
    }

    private static async Task<string> GetRawBodyAsync(HttpRequest request, Encoding? encoding = null)
    {
        request.Body.Position = 0;
        using var reader = new StreamReader(request.Body, encoding ?? Encoding.UTF8);
        string body = await reader.ReadToEndAsync().ConfigureAwait(false);
        request.Body.Position = 0;

        return body;
    }
}

public record ErrorResponse(string Message);