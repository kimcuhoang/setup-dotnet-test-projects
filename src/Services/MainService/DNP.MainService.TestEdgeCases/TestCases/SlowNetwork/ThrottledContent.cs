namespace DNP.MainService.TestEdgeCases.TestCases.SlowNetwork;

/// <summary>
/// ThrottledContent controls the actual bytes-per-second sent over the wire 
/// by overriding SerializeToStreamAsync — writing one fixed-size chunk then waiting 1 second before the next, 
/// which is what truly slows down the TCP send rate rather than just slowing down how fast the content is read into the buffer.
/// </summary>
public class ThrottledContent : HttpContent
{
    private readonly HttpContent _inner;
    private readonly int _bytesPerSecond;

    public ThrottledContent(HttpContent inner, int bytesPerSecond)
    {
        this._inner = inner;
        this._bytesPerSecond = bytesPerSecond;

        // Copy headers from original content
        foreach (var header in inner.Headers)
        {
            this.Headers.TryAddWithoutValidation(header.Key, header.Value);
        }
    }

    protected override async Task SerializeToStreamAsync(Stream stream, TransportContext? context)
    {
        var buffer = new byte[this._bytesPerSecond]; // one second worth of bytes
        var innerStream = await this._inner.ReadAsStreamAsync();

        int bytesRead;
        while ((bytesRead = await innerStream.ReadAsync(buffer)) > 0)
        {
            await stream.WriteAsync(buffer.AsMemory(0, bytesRead));
            await stream.FlushAsync();
            await Task.Delay(TimeSpan.FromSeconds(1)); // throttle to bytesPerSecond
        }
    }

    protected override bool TryComputeLength(out long length)
    {
        length = 0;
        return false; // unknown length forces chunked transfer
    }
}
