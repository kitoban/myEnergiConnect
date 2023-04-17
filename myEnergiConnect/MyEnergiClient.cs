using System.Net;
using Flurl;
using Flurl.Http;
using MyEnergiConnect.Exceptions;
using myEnergiConnect.Extensions;
using myEnergiConnect.Model.Enums;
using myEnergiConnect.Model.External;
using myEnergiConnect.Model.External.Eddi;
using MyEnergiConnect.Model.External.Shared;
using myEnergiConnect.Model.External.Zappi;
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
    private Dictionary<int, RawZappiSummary> _zappiSummaries;
    private Dictionary<int, RawEddiSummary> _eddiSummaries;

    public MyEnergiClient(string hubSerialNo, string apiKey)
    {
        _apiKey = apiKey;
        _hubSerialNo = hubSerialNo;

        _zappiSummaries = new Dictionary<int, RawZappiSummary>();
        _eddiSummaries = new Dictionary<int, RawEddiSummary>();
    }

    public async Task<ZappiSumary[]> GetZappiSummaries(PowerUnits powerUnit = PowerUnits.Watt, EnergyUnit energyUnit = EnergyUnit.KiloWattHour)
    {
        var allZappiSummary = await GetAllZappiSummariesInternal();
        return allZappiSummary.Zappis.Select(rawData => ConvertZappiSummary(rawData, powerUnit, energyUnit)).ToArray();
    }

    private async Task<AllZappiSummary> GetAllZappiSummariesInternal()
    {
        var url = await ResolveServerAddress("cgi-jstatus-Z");
        return await url
            .WithDigestAuth(_hubSerialNo, _apiKey)
            .GetJsonAsync<AllZappiSummary>();
    }

    public async Task<EddiSummary[]> GetEddiSummaries(PowerUnits powerUnit = PowerUnits.Watt, EnergyUnit energyUnit = EnergyUnit.KiloWattHour)
    {
        var allEddiSummary = await GetAllEddiSummariesInternal();
        return allEddiSummary.Eiddis.Select(rawData => ConvertEddiSummary(rawData, powerUnit, energyUnit)).ToArray();
    }

    private async Task<AllEddiSummary> GetAllEddiSummariesInternal()
    {
        var url = await ResolveServerAddress("cgi-jstatus-E");
        var allEddiSummary = await url
            .WithDigestAuth(_hubSerialNo, _apiKey)
            .GetJsonAsync<AllEddiSummary>();
        return allEddiSummary;
    }

    private EddiSummary ConvertEddiSummary(RawEddiSummary rawData, PowerUnits unit, EnergyUnit energyUnit)
    {
        return new EddiSummary(
            rawData.SerialNumber,
            DateTime.Parse($"{rawData.Date} {rawData.Time}"), 
            rawData.Ct1Name,
            rawData.Ct2Name,
            rawData.Ct3Name,
            ConvertFromWatt(rawData.PhysicalCt1ValueWatts, unit),
            ConvertFromWatt(rawData.PhysicalCt2ValueWatts, unit),
            ConvertFromWatt(rawData.PhysicalCt3ValueWatts, unit),
            ConvertFromWatt(rawData.GeneratedWatts, unit),
            ConvertFromWatt(rawData.CurrentWattsFromGrid, unit),
            ConvertFromWatt(rawData.DiversionAmountWatts, unit),
            rawData.Status,
            rawData.TemperatureProbe1,
            rawData.TemperatureProbe2,
            rawData.Heater1Name,
            rawData.Heater2Name,
            rawData.ActiveHeater,
            rawData.Priority,
            ConvertFromKilowattHour(rawData.TotalKWhTransferred, energyUnit));
    }

    private ZappiSumary ConvertZappiSummary(RawZappiSummary rawData, PowerUnits powerUnit, EnergyUnit energyUnit)
    {
        return new ZappiSumary(
            rawData.SerialNumber,
            DateTime.Parse($"{rawData.Date} {rawData.Time}"), 
            rawData.Ct1Name,
            rawData.Ct2Name,
            rawData.Ct3Name,
            ConvertFromWatt(rawData.PhysicalCt1ValueWatts, powerUnit),
            ConvertFromWatt(rawData.PhysicalCt2ValueWatts, powerUnit),
            ConvertFromWatt(rawData.PhysicalCt3ValueWatts, powerUnit),
            ConvertFromWatt(rawData.GeneratedWatts, powerUnit),
            ConvertFromWatt(rawData.WattsFromGrid, powerUnit),
            ConvertFromWatt(rawData.DiversionAmountWatts, powerUnit),
            rawData.Priority,
            rawData.UnitStatus,
            ConvertFromKilowattHour(rawData.ChargeAddedKWh, energyUnit),
            rawData.BoostManual,
            rawData.BoostTimed,
            rawData.BoostSmart,
            rawData.LockStatus,
            rawData.PlugStatus,
            rawData.Mode,
            rawData.MinimumGreenLevel,
            rawData.SmartBoostStartTimeHour,
            ConvertFromKilowattHour(rawData.SmartBoostKWhToAdd, energyUnit),
            rawData.SmartBoostStartTimeMinute,
            rawData.BoostHour,
            ConvertFromKilowattHour(rawData.BoostKWh, energyUnit),
            rawData.BoostMinute);
    }

    private decimal ConvertFromWatt(int value, PowerUnits unit)
    {
        return unit switch
        {
            PowerUnits.Watt => value,
            PowerUnits.KiloWatt => value.FromWattToKilowatt(),
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }
    
    private decimal ConvertFromKilowattHour(decimal value, EnergyUnit unit)
    {
        return unit switch
        {
            EnergyUnit.WattMinute => value.FromKiloWattHourToWattMinutes(),
            EnergyUnit.WattSecond => value.FromKiloWattHourToWattSeconds(),
            EnergyUnit.KiloWattHour => value,
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
        };
    }
    
    public async Task<HistoricDay> GetZappiHistory(int serialNo, int year, int month, int day, EnergyUnit unit = EnergyUnit.WattSecond)
    {
        var zappi = await GetZappiSummary(serialNo);
        var url = await ResolveServerAddress($"cgi-jday-Z{serialNo}-{year}-{month}-{day}");
        var history = await GetDayData(serialNo, url);
        
        return new HistoricDay(
            zappi.Ct2Name,
            zappi.Ct3Name,
            zappi.Ct4Name,
            unit,
            history.Select(mh => ConvertHistoricData(mh, unit)).ToArray());
    }

    private HistoricMinute ConvertHistoricData(MinuteHistory mh, EnergyUnit energyUnit)
    {
        var dateTime = DateTime.Parse($"{mh.Year}-{mh.Month:D2}-{mh.DayOfMonth:D2}T{mh.Hour:D2}:{mh.Minute:D2}:00Z");

        var zappiWattSeconds = mh.Phase1WattSeconds + mh.Phase2WattSeconds + mh.Phase3WattSeconds;
        var gridWattSeconds = mh.ImportedWattSeconds + (0 - mh.ExportedWattSeconds);
        var genWattSeconds = mh.PositiveGeneration + (0 - mh.NegativeGeneration);
        var ct1WattSeconds = mh.PositiveEnergyCt1 + (0 - mh.NegativeEnergyCt1);
        var ct2WattSeconds = mh.PositiveEnergyCt2 + (0 - mh.NegativeEnergyCt2);
        var ct3WattSeconds = mh.PositiveEnergyCt3 + (0 - mh.NegativeEnergyCt3);

        return new HistoricMinute(
            dateTime,
            gridWattSeconds,
            zappiWattSeconds,
            HandleUnitConversion(genWattSeconds, energyUnit),
            HandleUnitConversion(ct1WattSeconds, energyUnit),
            HandleUnitConversion(ct2WattSeconds, energyUnit),
            HandleUnitConversion(ct3WattSeconds, energyUnit));
    }

    private decimal HandleUnitConversion(int value, EnergyUnit unit)
    {
        return unit switch
        {
            EnergyUnit.WattSecond => value,
            EnergyUnit.WattMinute => value.FromWattSecondToToWattMinute(),
            EnergyUnit.KiloWattHour => value.FromWattSecondToToKiloWattHours(),
            _ => throw new ArgumentOutOfRangeException(nameof(unit), unit, null)
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

    private async Task<RawZappiSummary?> GetZappiSummary(int serialNo)
    {
        if (!_zappiSummaries.ContainsKey(serialNo))
        {
            var allZappiSummary = await GetAllZappiSummariesInternal();
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

    private async Task<RawEddiSummary?> GetEddiSummary(int serialNo)
    {
        if (!_eddiSummaries.ContainsKey(serialNo))
        {
            var allEddiSummaries = await GetAllEddiSummariesInternal();
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

    public async Task<HistoricDay> GetEddiHistory(int serialNo, int year, int month, int day, EnergyUnit unit)
    {
        var eddi = await GetEddiSummary(serialNo);
        var url = await ResolveServerAddress($"cgi-jday-E{serialNo}-{year}-{month}-{day}");
        var history = await GetDayData(serialNo, url);
        
        return new HistoricDay(
            eddi.Ct2Name,
            eddi.Ct3Name,
            "None",
            unit,
            history.Select(mh => ConvertHistoricData(mh, unit)).ToArray());
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