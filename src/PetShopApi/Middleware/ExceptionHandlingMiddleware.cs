using System.Net;
using System.Text.Json;
using PetShopApi.Exceptions;

namespace PetShopApi.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (NotFoundException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.NotFound, ex.Message);
        }
        catch (BusinessRuleException ex)
        {
            await WriteProblemAsync(context, HttpStatusCode.BadRequest, ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro nao tratado ao processar {Path}", context.Request.Path);
            await WriteProblemAsync(context, HttpStatusCode.InternalServerError, "Ocorreu um erro inesperado.");
        }
    }

    private static Task WriteProblemAsync(HttpContext context, HttpStatusCode statusCode, string message)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new
        {
            status = (int)statusCode,
            title = message,
            traceId = context.TraceIdentifier
        };

        return context.Response.WriteAsync(JsonSerializer.Serialize(problem));
    }
}
