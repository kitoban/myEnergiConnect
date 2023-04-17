using System.Runtime.Serialization;

namespace MyEnergiConnect.Model.External.Shared;

public enum FlowUnit
{
    [EnumMember(Value = "W")] Watt,
    [EnumMember(Value = "WM")] WattMinute,
    [EnumMember(Value = "KWH")] KiloWattHour
}