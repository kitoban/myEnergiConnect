﻿using myEnergiConnect.Model.Enums;
using Newtonsoft.Json;

namespace MyEnergiConnect.Model.Internal.Zappi;

internal record RawZappiSummary(
    [property:JsonProperty("sno")] int SerialNumber,
    [property:JsonProperty("dat")] string Date,
    [property:JsonProperty("tim")] string Time,
    [property:JsonProperty("ectp1")] int PhysicalCt1ValueWatts,
    [property:JsonProperty("ectp2")] int PhysicalCt2ValueWatts,
    [property:JsonProperty("ectp3")] int PhysicalCt3ValueWatts,
    [property:JsonProperty("ectt1")] string Ct1Name,
    [property:JsonProperty("ectt2")] string Ct2Name,
    [property:JsonProperty("ectt3")] string Ct3Name,
    [property:JsonProperty("bsm")] Enable BoostManual,
    [property:JsonProperty("bst")] Enable BoostTimed,
    [property:JsonProperty("cmt")] int CommandTimer,
    [property:JsonProperty("dst")] int UseDaylightSavingsTime,
    [property:JsonProperty("div")] int DiversionAmountWatts,
    [property:JsonProperty("frq")] decimal SupplyFrequency,
    [property:JsonProperty("fwv")] string FirmwareVersion,
    [property:JsonProperty("gen")] int GeneratedWatts,
    [property:JsonProperty("grd")] int WattsFromGrid,
    [property:JsonProperty("pha")] int Phases,
    [property:JsonProperty("pri")] int Priority,
    [property:JsonProperty("sta")] Status UnitStatus,
    [property:JsonProperty("tz")] int Timezone,
    [property:JsonProperty("vol")] int SupplyVoltage,
    [property:JsonProperty("che")] decimal ChargeAddedKWh,
    [property:JsonProperty("bss")] Enable BoostSmart,
    [property:JsonProperty("lck")] Lock LockStatus,
    [property:JsonProperty("pst")] PlugStatus PlugStatus,
    [property:JsonProperty("zmo")] Mode Mode,
    [property:JsonProperty("zs")] int Zs,
    [property:JsonProperty("ectp4")] int PhysicalCt4ValueWatts,
    [property:JsonProperty("ectt4")] string Ct4Name,
    [property:JsonProperty("ectt5")] string Ct5Name,
    [property:JsonProperty("ectt6")] string Ct6Name,
    [property:JsonProperty("ectp5")] int PhysicalCt5ValueWatts,
    [property:JsonProperty("ectp6")] int PhysicalCt6ValueWatts,
    [property:JsonProperty("mgl")] int MinimumGreenLevel,
    [property:JsonProperty("sbh")] int SmartBoostStartTimeHour,
    [property:JsonProperty("sbk")] decimal SmartBoostKWhToAdd,
    [property:JsonProperty("sbm")] int SmartBoostStartTimeMinute,
    [property:JsonProperty("tbh")] int BoostHour,
    [property:JsonProperty("tbk")] decimal BoostKWh,
    [property:JsonProperty("tbm")] int BoostMinute
);