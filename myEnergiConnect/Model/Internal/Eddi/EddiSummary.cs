using myEnergiConnect.Model.Internal.Shared;
using MyEnergiConnect.Model.Internal.Shared;
using Newtonsoft.Json;

namespace MyEnergiConnect.Model.Internal.Eddi;

public record EddiSummary(
    [property:JsonProperty("bsm")] int BoostMode,
    [property:JsonProperty("che")] double TotalKWhTransferred,
    [property:JsonProperty("cmt")] int CommandTimer,
    [property:JsonProperty("dat")] string Date,
    [property:JsonProperty("div")] int DiversionAmountWatts,
    [property:JsonProperty("dst")] int IsDaylightSavingsTimeEnabled,
    [property:JsonProperty("ectp1")] int PhysicalCt1ValueWatts,
    [property:JsonProperty("ectp2")] int PhysicalCt2ValueWatts,
    [property:JsonProperty("ectp3")] int PhysicalCt3ValueWatts,
    [property:JsonProperty("ectt1")] string Ct1Name,
    [property:JsonProperty("ectt2")] string Ct2Name,
    [property:JsonProperty("ectt3")] string Ct3Name,
    [property:JsonProperty("frq")] decimal SupplyFrequency,
    [property:JsonProperty("fwv")] string FirmwareVersion,
    [property:JsonProperty("gen")] int GeneratedWatts,
    [property:JsonProperty("grd")] int CurrentWattsFromGrid,
    [property:JsonProperty("hno")] int ActiveHeater,
    [property:JsonProperty("ht1")] string Heater1Name,
    [property:JsonProperty("ht2")] string Heater2Name,
    [property:JsonProperty("pha")] int PhaseNumber,
    [property:JsonProperty("pri")] int Priority,
    [property:JsonProperty("r1a")] int UnknownR1A,
    [property:JsonProperty("r2a")] int UnknownR2A,
    [property:JsonProperty("r2b")] int UnknownR2B,
    [property:JsonProperty("rbt")] int RemainingBoostTimeSeconds,
    [property:JsonProperty("sno")] int SerialNumber,
    [property:JsonProperty("sta")] Status Status,
    [property:JsonProperty("tim")] string Time,
    [property:JsonProperty("tp")] int TemperatureProbe1,
    [property:JsonProperty("tp2")] int TemperatureProbe2,
    [property:JsonProperty("vol")] int VoltageOut
) : IMyEnergiProduct;
