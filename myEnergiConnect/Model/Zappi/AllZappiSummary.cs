using Newtonsoft.Json;

namespace myEnergiConnect.Model.Zappi;

public record AllZappiSummary(
    [property:JsonProperty("zappi")] ZappiSummary[] Zappis);