
using Microsoft.AspNetCore.Mvc;

namespace DNP.ThisIsAMainService.Features.People.Create;

public static class CreatePeopleEndpoint
{
    public static readonly string Route = @"/";

    extension(RouteGroupBuilder builder)
    {
        public RouteGroupBuilder AddCreatePersonEndpoint()
        {
            builder.MapPost(Route, async(
                [FromBody] CreatePersonPayload payload,
                [FromServices] CreatePeopleCommandHandler handler,
                CancellationToken cancellationToken = default
            ) =>
            {
                var command = new CreatePeopleCommand(payload);
                var result = await handler.Handle(command, cancellationToken);
                return result.ToTypedResult(() => TypedResults.Created());
            })
            .Produces(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status406NotAcceptable)
            .Produces(StatusCodes.Status500InternalServerError);
            
            return builder;
        }
    }
}