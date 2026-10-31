using Microsoft.AspNetCore.Diagnostics;

namespace DNP.ThisIsAMainService.Shared.Infrastructures.ExceptionHandlers;

public class DefaultExceptionHandler(ILogger<DefaultExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(exception, "An unexpected exception occurred while processing the request.");

        await TypedResults.Problem(
            // type: exception.GetType().Name,
            statusCode: StatusCodes.Status500InternalServerError,
            title: "An unexpected error occurred while processing the request.",
            detail: "An unexpected error occurred while processing the request. Please try again later.",
            extensions: new Dictionary<string, object?>
            {
                ["errors"] = new[] { exception.Message }
            }
        ).ExecuteAsync(httpContext);

        return true;
    }
}