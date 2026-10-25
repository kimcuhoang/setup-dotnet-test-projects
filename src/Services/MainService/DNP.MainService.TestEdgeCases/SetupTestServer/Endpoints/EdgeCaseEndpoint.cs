using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace DNP.MainService.TestEdgeCases.SetupTestServer.Endpoints;

public record PostDataModel(string Message);

public static class EdgeCaseEndpoint
{
    extension(IEndpointRouteBuilder endpoint)
    {
        public IEndpointRouteBuilder AddSlowNetwork()
        {
            var group = endpoint.MapGroup("/integration-tests");

            group.MapPost("/slow-network-simulator", ([FromBody] PostDataModel payload) =>
            {
                return TypedResults.Ok(payload.Message);
            });

            return endpoint;
        }
    }
}
