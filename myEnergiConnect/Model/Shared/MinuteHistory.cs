using Newtonsoft.Json;

namespace myEnergiConnect.Model.Shared;

public class MinuteHistory
{
    [JsonProperty("min")] public int Minute { get; init; }
    [JsonProperty("hr")] public int Hour { get; init; }
    [JsonProperty("dom")] public int DayOfMonth { get; init; }
    [JsonProperty("mon")] public int Month { get; init; }
    [JsonProperty("yr")] public int Year { get; init; }
    [JsonProperty("dow")] public string DayOfWeek { get; init; }
    
    [JsonProperty("imp")] public int ImportedJoules { get; init; }
    [JsonProperty("exp")] public int ExportedJoules { get; init; }
    
    [JsonProperty("gen")] public int NegativeGeneration { get; init; }
    [JsonProperty("gep")] public int PositiveGeneration { get; init; }
    
    [JsonProperty("v1")] public int Voltage { get; init; }
    [JsonProperty("frq")] public int Frequency { get; init; }
    
    [JsonProperty("nect1")] public int NegativeEnergyCt1 { get; init; }
    [JsonProperty("nect2")] public int NegativeEnergyCt2 { get; init; }
    [JsonProperty("nect3")] public int NegativeEnergyCt3 { get; init; }
    [JsonProperty("pect1")] public int PositiveEnergyCt1 { get; init; }
    [JsonProperty("pect2")] public int PositiveEnergyCt2 { get; init; }
    [JsonProperty("pect3")] public int PositiveEnergyCt3 { get; init; }
    
    [JsonProperty("h1d")] public int Phase1Joules { get; init; }
    [JsonProperty("h2d")] public int Phase2Joules { get; init; }
    [JsonProperty("h3d")] public int Phase3Joules { get; init; }
}