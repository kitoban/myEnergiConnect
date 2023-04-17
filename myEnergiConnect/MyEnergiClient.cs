using System.Net;
using Flurl;
using Flurl.Http;
using MyEnergiConnect.Exceptions;
using MyEnergiConnect.Model.External.Shared;
using MyEnergiConnect.Model.Internal.Eddi;
using MyEnergiConnect.Model.Internal.Shared;
using MyEnergiConnect.Model.Internal.Zappi;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace MyEnergiConnect;

public class MyEnergiClient : IMyEnergiClient
{
    private Url? _serverAddress = null;
    private readonly string _hubSerialNo;
    private readonly string _apiKey;
    private Dictionary<int, ZappiSummary> _zappiSummaries;

    public MyEnergiClient(string hubSerialNo, string apiKey)
    {
        _apiKey = apiKey;
        _hubSerialNo = hubSerialNo;

        _zappiSummaries = new Dictionary<int, ZappiSummary>();
    }

    public async Task<AllZappiSummary> GetZappiSummaries()
    {
        var url = await ResolveServerAddress("cgi-jstatus-Z");
        return await url
            .WithDigestAuth(_hubSerialNo,_apiKey)
            .GetJsonAsync<AllZappiSummary>();
    }
    
    public async Task<AllEddiSummary> GetEddiSummaries()
    {
        var url = await ResolveServerAddress("cgi-jstatus-E");
        return await url
            .WithDigestAuth(_hubSerialNo,_apiKey)
            .GetJsonAsync<AllEddiSummary>();
    }

    public async Task<HistoricDay> GetZappiHistory(int serialNo, int year, int month, int day, FlowUnit flowUnit = FlowUnit.JouleSeconds)
    {
        var zappi = await GetZappiSummary(serialNo);
        var url = await ResolveServerAddress($"cgi-jday-Z{serialNo}-{year}-{month}-{day}");
        var history = await GetDayData(serialNo, url);
        
        return new HistoricDay(
            zappi.Ct2Name,
            zappi.Ct3Name,
            zappi.Ct4Name,
            flowUnit,
            history.Select(mh => ConvertHistoricData(mh, flowUnit)).ToArray());
    }

    private HistoricMinute ConvertHistoricData(MinuteHistory mh, FlowUnit flowUnit)
    {
        var dateTime = DateTime.Parse($"{mh.Year}-{mh.Month:D2}-{mh.DayOfMonth:D2}T{mh.Hour:D2}:{mh.Minute:D2}:00Z");

        var zappiJoules = mh.Phase1Joules + mh.Phase2Joules + mh.Phase3Joules;
        var gridJoules = mh.ImportedJoules + (0 - mh.ExportedJoules);
        var genJoules = mh.PositiveGeneration + (0 - mh.NegativeGeneration);
        var ct1Joules = mh.PositiveEnergyCt1 + (0 - mh.NegativeEnergyCt1);
        var ct2Joules = mh.PositiveEnergyCt2 + (0 - mh.NegativeEnergyCt2);
        var ct3Joules = mh.PositiveEnergyCt3 + (0 - mh.NegativeEnergyCt3);

        var wattConversion = 60d;
        var kiloWattConversion = 60000d;
        
        return flowUnit switch
        {
            FlowUnit.JouleSeconds => new HistoricMinute(
                dateTime,
                gridJoules, 
                zappiJoules,
                genJoules, 
                ct1Joules, 
                ct2Joules, 
                ct3Joules),
            
            FlowUnit.Watt => new HistoricMinute(
                dateTime, 
                gridJoules/wattConversion, 
                zappiJoules/wattConversion,
                genJoules/wattConversion, 
                ct1Joules/wattConversion, 
                ct2Joules/wattConversion, 
                ct3Joules/wattConversion),
            
            FlowUnit.KiloWatt => new HistoricMinute(
                dateTime, 
                gridJoules/kiloWattConversion, 
                zappiJoules/kiloWattConversion,
                genJoules/kiloWattConversion, 
                ct1Joules/kiloWattConversion, 
                ct2Joules/kiloWattConversion, 
                ct3Joules/kiloWattConversion),
            
            _ => throw new ArgumentOutOfRangeException(nameof(flowUnit), flowUnit, null)
        };
    }

    private async Task<MinuteHistory[]> GetDayData(int serialNo, string url)
    {
        var response = await url
            .WithDigestAuth(_hubSerialNo, _apiKey)
            .GetAsync();

        var data = await response.ResponseMessage.Content.ReadAsStringAsync();

        var converter = new ExpandoObjectConverter();
        dynamic message = JsonConvert.DeserializeObject<JObject>(data, converter);
        var historicDataArray = message[$"U{serialNo}"];
        var history = JsonConvert.DeserializeObject<MinuteHistory[]>(historicDataArray.ToString());
        return history;
    }

    private async Task<ZappiSummary?> GetZappiSummary(int serialNo)
    {
        if (!_zappiSummaries.ContainsKey(serialNo))
        {
            var allZappiSummary = await GetZappiSummaries();
            foreach (var z in allZappiSummary.Zappis)
            {
                _zappiSummaries.Add(z.SerialNumber, z);
            }
        }

        if (!_zappiSummaries.TryGetValue(serialNo, out var zappi))
        {
            throw new ItemNotFoundException(serialNo, MyEnergiProduct.Zappi);
        }

        return zappi;
    }

    public async Task<MinuteHistory[]> GetEddiHistory(int serialNo, int year, int month, int day)
    {
        var url = await ResolveServerAddress($"cgi-jday-E{serialNo}-{year}-{month}-{day}");
        return await GetDayData(serialNo, url);
    }

    private async Task<string> ResolveServerAddress(string target)
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