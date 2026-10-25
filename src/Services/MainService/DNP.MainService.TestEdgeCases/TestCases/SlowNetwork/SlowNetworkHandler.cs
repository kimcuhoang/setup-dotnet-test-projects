namespace DNP.MainService.TestEdgeCases.TestCases.SlowNetwork;

/// <summary>
/// SlowNetworkHandler is a DelegatingHandler that intercepts the HttpClient request pipeline before it hits the socket, 
/// replacing the original HttpContent with a ThrottledContent wrapper 
/// so every outgoing request is automatically throttled without changing any test code.
/// </summary>
/// <param name="bytesPerSecond"></param>
public class SlowNetworkHandler(int bytesPerSecond) : DelegatingHandler(new HttpClientHandler())
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content != null)
        {
            var originalContent = request.Content;
            request.Content = new ThrottledContent(originalContent, bytesPerSecond);
        }

        return await base.SendAsync(request, cancellationToken);
    }
}
