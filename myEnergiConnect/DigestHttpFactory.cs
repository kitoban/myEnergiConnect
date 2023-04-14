using System.Net;

namespace myEnergiConnect;

/// <summary>
/// Custom factory to generate HttpClient and handler to use digest authentication and a continuous stream
/// </summary>
internal class DigestHttpFactory : Flurl.Http.Configuration.DefaultHttpClientFactory
{
    private CredentialCache CredCache { get; set; }

    public DigestHttpFactory(string url, string username, string password) : base()
    {
        Url = url;

        CredCache = new CredentialCache
        {
            { new Uri(Url), "Digest", new NetworkCredential(username, password) }
        };
    }

    private string Url { get; set; }

    public override HttpClient CreateHttpClient(HttpMessageHandler handler)
    {
        var client = base.CreateHttpClient(handler);
        client.Timeout = TimeSpan.FromMilliseconds(Timeout.Infinite); // To keep stream open indefinietly
        return client;
    }

    public override HttpMessageHandler CreateMessageHandler()
    {
        var handler = new HttpClientHandler
        {
            Credentials = CredCache.GetCredential(new Uri(Url), "Digest")
        };


        return handler;
    }
}