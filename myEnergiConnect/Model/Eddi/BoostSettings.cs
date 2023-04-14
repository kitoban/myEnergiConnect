using Newtonsoft.Json;

namespace myEnergiConnect.Model.Eddi;

public record BoostSettings(
    [property:JsonProperty("slt")] int Slot,
    [property:JsonProperty("bsh")] int BoostStartHour,
    [property:JsonProperty("bsm")] int BoostStartMinute,
    [property:JsonProperty("bdh")] int BoostDurationHour,
    [property:JsonProperty("bdm")] int BoostDurationMinute,
    [property:JsonProperty("bdd")] string BoostDaysOfWeek
);