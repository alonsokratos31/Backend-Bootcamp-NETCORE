using System;

namespace GameStore.Api.Features.Diagnostics.GetNodeInfo;

public static class GetNodeInfoEndpoint
{
    public static void MapGetNodeInfo(this IEndpointRouteBuilder app)
    {
        app.MapGet("/nodeinfo", () =>
        {
            var nodeInfo = new
            {
                MachineName = Environment.MachineName,
                OSVersion = Environment.OSVersion.ToString(),
                ProcessorCount = Environment.ProcessorCount,
                FrameworkDescription = System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription,
                CurrentDirectory = Environment.CurrentDirectory,
                Uptime = (DateTime.UtcNow - System.Diagnostics.Process.GetCurrentProcess().StartTime.ToUniversalTime()).ToString()
            };

            return Results.Ok(nodeInfo);
        })
        .WithTags("Diagnostics")
        .WithName("GetNodeInfo")
        .WithSummary("Retrieves information about the current server node.");
    }
}
