using System.Runtime.Serialization;

namespace MyEnergiConnect.Model.External.Shared;

public enum FlowUnit
{
    [EnumMember(Value = "J")] Joules,
    [EnumMember(Value = "W")] Watt,
    [EnumMember(Value = "KW")] KiloWatt
}