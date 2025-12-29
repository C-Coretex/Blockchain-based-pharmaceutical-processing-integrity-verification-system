namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.GetDetails
{
    public static class Endpoint
    {
        public static void Map(WebApplication app)
        {
            app.MapGet("/protocol/getDetails", async (
                [AsParameters] Request request,
                Handler handler,
                CancellationToken ct) =>
            {
                if (!Validator.IsValid(request, out var error))
                    return Results.BadRequest(error);

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
            .Produces<Response>(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest);
        }
    }
}
