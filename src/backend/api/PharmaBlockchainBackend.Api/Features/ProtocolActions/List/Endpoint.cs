namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.List
{
    public static class Endpoint
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/protocol/list", async (
                [AsParameters] Request request,
                Handler handler,
                CancellationToken ct) =>
            {
                try
                {
                    var response = await handler.Handle(request, ct);
                    return Results.Ok(response);
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .Produces<IAsyncEnumerable<Response>>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest);
        }
    }
}
