using System.Text.Json;

namespace GarasTransport.Api.Common;

// Catch-all → returns the documented error envelope. Any unhandled exception
// becomes Err10 with the raw message, at HTTP 200 (the old backend leaked
// ex.Message under Err10 — kept faithful, but never a 500 to the clients).
public class ExceptionEnvelopeMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionEnvelopeMiddleware> _logger;

    public ExceptionEnvelopeMiddleware(RequestDelegate next, ILogger<ExceptionEnvelopeMiddleware> logger)
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
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            if (context.Response.HasStarted) throw;
            context.Response.Clear();
            context.Response.StatusCode = 200;
            context.Response.ContentType = "application/json";
            var body = Envelope.Fail("Err10", ex.Message);
            await context.Response.WriteAsync(JsonSerializer.Serialize(body));
        }
    }
}
