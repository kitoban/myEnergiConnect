using System.Runtime.Serialization;

namespace myEnergiConnect.Model.Enums;

public enum EnergyUnit
{
    [EnumMember(Value = "WS")] WattSecond,
    [EnumMember(Value = "WM")] WattMinute,
    [EnumMember(Value = "KWH")] KiloWattHour
}