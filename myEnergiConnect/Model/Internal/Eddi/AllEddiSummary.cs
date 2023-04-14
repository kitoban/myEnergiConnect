using Newtonsoft.Json;

namespace MyEnergiConnect.Model.Internal.Eddi;

public record AllEddiSummary(
    [property:JsonProperty("eddi")] EddiSummary[] Eiddis);