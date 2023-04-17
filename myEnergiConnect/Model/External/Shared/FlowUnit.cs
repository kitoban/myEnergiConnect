using System.Runtime.Serialization;

namespace MyEnergiConnect.Model.External.Shared;

public enum FlowUnit
{
    [EnumMember(Value = "JS")] JouleSeconds,
    [EnumMember(Value = "W")] Watt,
    [EnumMember(Value = "KW")] KiloWatt
}