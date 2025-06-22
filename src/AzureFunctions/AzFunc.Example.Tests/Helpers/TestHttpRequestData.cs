using Microsoft.Azure.Functions.Worker.Http;
using System.Security.Claims;
using System.Text;

namespace AzFunc.Example.Tests.Helpers;
public class TestHttpRequestData(string urlPath, string method, string? jsonBody = null) : HttpRequestData(new TestFunctionContext())
{
    public override Uri Url { get; } = new Uri($"http://localhost{urlPath}");
    public override string Method { get; } = method;
    public override Stream Body { get; } = string.IsNullOrWhiteSpace(jsonBody)
                                    ? Stream.Null
                                    : new MemoryStream(Encoding.UTF8.GetBytes(jsonBody));
    public override HttpHeadersCollection Headers => [];
    public override IReadOnlyCollection<IHttpCookie> Cookies { get; } = [];
    public override IEnumerable<ClaimsIdentity> Identities { get; } = [];

    public override HttpResponseData CreateResponse()
    {
        return new TestHttpResponseData(this.FunctionContext);
    }
}
