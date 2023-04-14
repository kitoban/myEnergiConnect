using Newtonsoft.Json;

namespace MyEnergiConnect.Model.Internal.Zappi;

public record AllZappiSummary(
    [property:JsonProperty("zappi")] ZappiSummary[] Zappis);