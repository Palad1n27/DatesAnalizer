using System.Net;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi.Middlewares;

public class ExceptionHandlerMiddlewareV1
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly IHostEnvironment _env;

    public ExceptionHandlerMiddlewareV1(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger, IHostEnvironment env)
    {
        _next = next;
        _logger = logger;
        _env = env;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception occurred.");

            var response = context.Response;
            response.ContentType = "application/json";

            var errorDetails = new ErrorDetails
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = "An unexpected error occurred."
            };

            switch (ex)
            {
                case ArgumentException or ArgumentNullException:
                    errorDetails.StatusCode = StatusCodes.Status400BadRequest;
                    errorDetails.Message = "Invalid input.";
                    break;
            }

            if (_env.IsDevelopment())
            {
                errorDetails.StackTrace = ex.StackTrace;
                errorDetails.Message = ex.Message;
            }

            response.StatusCode = errorDetails.StatusCode;
            await context.Response.WriteAsJsonAsync(errorDetails);
        }
    }
}
public class ErrorDetails
{
    public int StatusCode { get; set; }
    public string? Message { get; set; }
    public string? StackTrace { get; set; }
}