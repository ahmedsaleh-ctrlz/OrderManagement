using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(
        RequestDelegate next,
        ILogger<ExceptionMiddleware> logger)
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
            await HandleException(context, ex, HttpStatusCode.NotFound);
        }
        catch (BadRequestException ex)
        {
            await HandleException(context, ex, HttpStatusCode.BadRequest);
        }
        catch (ForbiddenException ex)
        {
            await HandleException(context, ex, HttpStatusCode.Forbidden);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            _logger.LogWarning(ex, "Concurrency conflict occurred.");

            await HandleException(
                context,
                new Exception("The resource was modified by another request. Please retry."),
                HttpStatusCode.Conflict);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Database update exception occurred.");

            await HandleException(
                context,
                new Exception("Database constraint violation."),
                HttpStatusCode.Conflict);
        }


        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled Exception");

            await HandleException(
                context,
                new Exception("An unexpected error occurred."),
                HttpStatusCode.InternalServerError);
        }
    }

    private static async Task HandleException(
    HttpContext context,
    Exception ex,
    HttpStatusCode statusCode)
    {
        context.Response.ContentType = "application/problem+json";
        context.Response.StatusCode = (int)statusCode;

        var problem = new ProblemDetails
        {
            Type = GetTypeUrl(statusCode),
            Title = GetTitle(statusCode),
            Status = (int)statusCode,
            Detail = ex.Message,
            Instance = context.Request.Path
        };

        problem.Extensions["traceId"] = context.TraceIdentifier;

        await context.Response.WriteAsJsonAsync(problem);
    }


    private static string GetTitle(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.NotFound => "Resource Not Found",
        HttpStatusCode.BadRequest => "Bad Request",
        HttpStatusCode.Forbidden => "Forbidden",
        HttpStatusCode.Conflict => "Conflict",
        _ => "Internal Server Error"
    };

    private static string GetTypeUrl(HttpStatusCode statusCode) => statusCode switch
    {
        HttpStatusCode.NotFound => "https://httpstatuses.com/404",
        HttpStatusCode.BadRequest => "https://httpstatuses.com/400",
        HttpStatusCode.Forbidden => "https://httpstatuses.com/403",
        HttpStatusCode.Conflict => "https://httpstatuses.com/409",
        _ => "https://httpstatuses.com/500"
    };
}
