using System.Net;

namespace MyEnergiConnect;

/// <summary>
/// Custom factory to generate HttpClient and handler to use digest authentication and a continuous stream
/// https://stackoverflow.com/questions/52223522/httpclienthandler-rfc-7616-digest-authentication-header-using-wrong-uri
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