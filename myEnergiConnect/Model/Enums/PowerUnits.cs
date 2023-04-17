using System.Runtime.Serialization;

namespace myEnergiConnect.Model.Enums;

public enum PowerUnits
{
    [EnumMember(Value = "W")] Watt,
    [EnumMember(Value = "KW")] KiloWatt
}