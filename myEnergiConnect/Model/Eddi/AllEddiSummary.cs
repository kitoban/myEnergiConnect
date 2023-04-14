using Newtonsoft.Json;

namespace myEnergiConnect.Model.Eddi;

public record AllEddiSummary(
    [property:JsonProperty("eddi")] EddiSummary[] Eiddis);