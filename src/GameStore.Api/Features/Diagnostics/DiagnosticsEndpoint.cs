using System;
using GameStore.Api.Features.Diagnostics.GetNodeInfo;

namespace GameStore.Api.Features.Diagnostics;

public static class DiagnosticsEndpoint
{
    public static void MapDiagnostics(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/diagnostics");
        group.MapGetNodeInfo();
    }
}
