using System.Runtime.Serialization;

namespace MyEnergiConnect.Model.External.Shared;

public enum EnergyUnit
{
    [EnumMember(Value = "WS")] WattSecond,
    [EnumMember(Value = "WM")] WattMinute,
    [EnumMember(Value = "KWH")] KiloWattHour
}