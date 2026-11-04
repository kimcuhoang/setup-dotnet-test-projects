using Microsoft.AspNetCore.Mvc;

namespace DNP.ThisIsAMainService.Features.People.ImportMinimal;

public static class ImportMinimalEndpoint
{
    public static readonly string Route = "/import-minimal";

    extension(RouteGroupBuilder builder)
    {
        public RouteGroupBuilder AddImportMinimalEndpoint()
        {
            builder.MapPost(Route, async(
                [AsParameters] ImportMinimalPayload payload,
                [FromServices] ImportMinimalHandler handler,
                CancellationToken cancellationToken = default
            ) =>
            {
                var result = await handler.Handle(payload, cancellationToken);
                return result.ToTypedResult(value => TypedResults.Ok(value));
            })
            .Produces<List<ImportMinimalPersonModel>>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status406NotAcceptable)
            .Produces(StatusCodes.Status500InternalServerError)
            .DisableAntiforgery();

            return builder;
        }
    }
}