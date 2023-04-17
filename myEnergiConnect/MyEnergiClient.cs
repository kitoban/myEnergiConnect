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
    private Dictionary<int, EddiSummary> _eddiSummaries;

    public MyEnergiClient(string hubSerialNo, string apiKey)
    {
        _apiKey = apiKey;
        _hubSerialNo = hubSerialNo;

        _zappiSummaries = new Dictionary<int, ZappiSummary>();
        _eddiSummaries = new Dictionary<int, EddiSummary>();
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

    public async Task<HistoricDay> GetZappiHistory(int serialNo, int year, int month, int day, FlowUnit flowUnit = FlowUnit.Watt)
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

        var wattMinuteConversion = 60d;
        var kiloWattHourConversion = 3_600_000d;
        
        return flowUnit switch
        {
            FlowUnit.Watt => new HistoricMinute(
                dateTime,
                gridJoules, 
                zappiJoules,
                genJoules, 
                ct1Joules, 
                ct2Joules, 
                ct3Joules),
            
            FlowUnit.WattMinute => new HistoricMinute(
                dateTime, 
                gridJoules/wattMinuteConversion, 
                zappiJoules/wattMinuteConversion,
                genJoules/wattMinuteConversion, 
                ct1Joules/wattMinuteConversion, 
                ct2Joules/wattMinuteConversion, 
                ct3Joules/wattMinuteConversion),
            
            FlowUnit.KiloWattHour => new HistoricMinute(
                dateTime, 
                gridJoules/kiloWattHourConversion, 
                zappiJoules/kiloWattHourConversion,
                genJoules/kiloWattHourConversion, 
                ct1Joules/kiloWattHourConversion, 
                ct2Joules/kiloWattHourConversion, 
                ct3Joules/kiloWattHourConversion),
            
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

    private async Task<EddiSummary?> GetEddiSummary(int serialNo)
    {
        if (!_eddiSummaries.ContainsKey(serialNo))
        {
            var allEddiSummaries = await GetEddiSummaries();
            foreach (var e in allEddiSummaries.Eiddis)
            {
                _eddiSummaries.Add(e.SerialNumber, e);
            }
        }

        if (!_eddiSummaries.TryGetValue(serialNo, out var eddi))
        {
            throw new ItemNotFoundException(serialNo, MyEnergiProduct.Eddi);
        }

        return eddi;
    }

    public async Task<HistoricDay> GetEddiHistory(int serialNo, int year, int month, int day, FlowUnit flowUnit)
    {
        var eddi = await GetEddiSummary(serialNo);
        var url = await ResolveServerAddress($"cgi-jday-E{serialNo}-{year}-{month}-{day}");
        var history = await GetDayData(serialNo, url);
        
        return new HistoricDay(
            eddi.Ct2Name,
            eddi.Ct3Name,
            "None",
            flowUnit,
            history.Select(mh => ConvertHistoricData(mh, flowUnit)).ToArray());
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