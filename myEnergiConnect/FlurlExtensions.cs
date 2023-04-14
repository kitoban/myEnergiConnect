﻿using Flurl.Http;

namespace MyEnergiConnect;

internal static class FlurlExtensions
{
    public static IFlurlRequest WithDigestAuth(this string url, string username, string password)
    {
        FlurlHttp.ConfigureClient(url, client =>
        {
            client.Configure(settings =>
            {
                settings.HttpClientFactory = new DigestHttpFactory(url, username, password);
            });
        });
        return new FlurlRequest(url);
    }
}