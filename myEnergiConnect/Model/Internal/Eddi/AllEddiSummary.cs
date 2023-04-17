using Newtonsoft.Json;

namespace MyEnergiConnect.Model.Internal.Eddi;

internal record AllEddiSummary(
    [property:JsonProperty("eddi")] RawEddiSummary[] Eiddis);