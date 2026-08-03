namespace DNP.PeopleService.Tests.xUnitV3.Infrastructure.HttpDelegatingHandlers;

internal class HttpLoggerDelegatingHandler(ITestOutputHelper testOutputHelper) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        testOutputHelper.WriteLine($"Request: {request.Method} {request.RequestUri}");
        if (request.Content is not null and not StreamContent)
        {
            var requestContent = await request.Content.ReadAsStringAsync(cancellationToken);
            testOutputHelper.WriteLine($"Request Content: {requestContent}");
        }

        var response = await base.SendAsync(request, cancellationToken);

        testOutputHelper.WriteLine($"Response: {(int)response.StatusCode} {response.ReasonPhrase}");
        if (response.Content is not null)
        {
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            testOutputHelper.WriteLine($"Response Content: {responseContent}");
        }

        return response;
    }
}
