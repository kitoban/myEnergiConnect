using System.Net;
using Flurl;
using Flurl.Http;
using myEnergiConnect.Exceptions;
using myEnergiConnect.Model.Shared;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace myEnergiConnect;

public class MyEnergiMyEnergiClient : IMyEnergiClient
{
    private Url? _serverAddress = null;
    private readonly string _hubSerialNo;
    private readonly string _apiKey;

    public MyEnergiMyEnergiClient(string hubSerialNo, string apiKey)
    {
        _apiKey = apiKey;
        _hubSerialNo = hubSerialNo;
    }

    public async Task<Model.Zappi.AllZappiSummary> GetZappiSummary()
    {
        var url = await GetServerAddress("cgi-jstatus-Z");
        return await url
            .WithDigestAuth(_hubSerialNo,_apiKey)
            .GetJsonAsync<Model.Zappi.AllZappiSummary>();
    }
    
    public async Task<Model.Eddi.AllEddiSummary> GetEddiSummary()
    {
        var url = await GetServerAddress("cgi-jstatus-E");
        return await url
            .WithDigestAuth(_hubSerialNo,_apiKey)
            .GetJsonAsync<Model.Eddi.AllEddiSummary>();
    }

    public async Task<MinuteHistory[]> GetZappiHistory(int serialNo, int year, int month, int day)
    {
        var url = await GetServerAddress($"cgi-jday-Z{serialNo}-{year}-{month}-{day}");
        var response = await url
            .WithDigestAuth(_hubSerialNo,_apiKey)
            .GetAsync();

        var data = await response.ResponseMessage.Content.ReadAsStringAsync();
        
        var converter = new ExpandoObjectConverter();
        dynamic message = JsonConvert.DeserializeObject<JObject>(data, converter);
        var historicDataArray = message[$"U{serialNo}"];
        return JsonConvert.DeserializeObject<MinuteHistory[]>(historicDataArray.ToString());
    }

    public async Task<MinuteHistory[]> GetEddiHistory(int serialNo, int year, int month, int day)
    {
        var url = await GetServerAddress($"cgi-jday-E{serialNo}-{year}-{month}-{day}");
        var response = await url
            .WithDigestAuth(_hubSerialNo,_apiKey)
            .GetAsync();

        var data = await response.ResponseMessage.Content.ReadAsStringAsync();
        
        var converter = new ExpandoObjectConverter();
        dynamic message = JsonConvert.DeserializeObject<JObject>(data, converter);
        var historicDataArray = message[$"U{serialNo}"];
        return JsonConvert.DeserializeObject<MinuteHistory[]>(historicDataArray.ToString());
    }

    private async Task<string> GetServerAddress(string target)
    {
        if (_serverAddress != null)
        {
            return Url.Combine($"https://{_serverAddress}", target);
        }

        var directorResponse = await MyEnergi.DirectorUrl
            .WithDigestAuth(_hubSerialNo, _apiKey)
            .GetAsync();

        if (directorResponse.StatusCode != (int)HttpStatusCode.OK)
        {
            throw new DirectorResponseException(
                directorResponse.StatusCode,
                directorResponse.ResponseMessage.ReasonPhrase);
        }

        var serverAddress = directorResponse.Headers.FirstOrDefault(i => i.Name == "x_myenergi-asn").Value;

        if (serverAddress == null)
        {
            throw new DirectorResponseException(directorResponse.StatusCode, "Missing x_myenergi-asn value");
        }

        _serverAddress = Url.Parse(serverAddress);
        return Url.Combine($"https://{_serverAddress}", target);
    }
}