using System;
using System.Diagnostics;

namespace GameStore.Api.Shared.Timing;

public class RequestTimmingMiddleware(RequestDelegate next, ILogger<RequestTimmingMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        Stopwatch stopwatch = new Stopwatch();

        try
        {
            stopwatch.Start();
            await next(context);
        }
        finally
        {
            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;
            logger.LogInformation("{Requestmethod} {RequestPath} comnpleted with status {Status} in {Elapsed}ms",
                                     context.Request.Method, context.Request.Path, context.Response.StatusCode, elapsed);
        }
    }

}
