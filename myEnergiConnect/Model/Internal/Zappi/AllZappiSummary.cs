using Newtonsoft.Json;

namespace MyEnergiConnect.Model.Internal.Zappi;

internal record AllZappiSummary(
    [property:JsonProperty("zappi")] RawZappiSummary[] Zappis);