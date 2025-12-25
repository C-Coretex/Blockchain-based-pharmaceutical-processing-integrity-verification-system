namespace PharmaBlockchainBackend.Api.Features.ProtocolActions.StepSubmit
{
    public static class Endpoint
    {
        public static void Map(WebApplication app)
        {
            app.MapPost("/protocol/stepSubmit", async (
                Request request,
                Handler handler,
                CancellationToken ct) =>
            {
                if (!Validator.IsValid(request, out var error))
                    return Results.BadRequest(error);

                try
                {
                    await handler.Handle(request, ct);
                    return Results.Ok();
                }
                catch (Exception ex)
                {
                    return Results.BadRequest(ex.Message);
                }
            })
            .Produces(StatusCodes.Status200OK)
            .Produces<string>(StatusCodes.Status400BadRequest);
        }
    }
}
