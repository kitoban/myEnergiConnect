using Newtonsoft.Json;

namespace MyEnergiConnect.Model.Internal.Shared;

internal record MinuteHistory(
    [property:JsonProperty("min")] int Minute,
    [property:JsonProperty("hr")] int Hour,
    [property:JsonProperty("dom")] int DayOfMonth,
    [property:JsonProperty("mon")] int Month,
    [property:JsonProperty("yr")] int Year,
    [property:JsonProperty("dow")] string DayOfWeek,
    [property:JsonProperty("imp")] int ImportedWattSeconds,
    [property:JsonProperty("exp")] int ExportedWattSeconds,
    [property:JsonProperty("gen")] int NegativeGeneration,
    [property:JsonProperty("gep")] int PositiveGeneration,
    [property:JsonProperty("v1")] int Voltage,
    [property:JsonProperty("frq")] int Frequency,
    [property:JsonProperty("nect1")] int NegativeEnergyCt1,
    [property:JsonProperty("nect2")] int NegativeEnergyCt2,
    [property:JsonProperty("nect3")] int NegativeEnergyCt3,
    [property:JsonProperty("pect1")] int PositiveEnergyCt1,
    [property:JsonProperty("pect2")] int PositiveEnergyCt2,
    [property:JsonProperty("pect3")] int PositiveEnergyCt3,
    [property:JsonProperty("h1d")] int Phase1WattSeconds,
    [property:JsonProperty("h2d")] int Phase2WattSeconds,
    [property:JsonProperty("h3d")] int Phase3WattSeconds);