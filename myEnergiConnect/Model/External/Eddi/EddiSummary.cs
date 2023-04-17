using myEnergiConnect.Model.Enums;
using myEnergiConnect.Model.External.Shared;

namespace myEnergiConnect.Model.External.Eddi;

public record EddiSummary(
    int SerialNumber,
    DateTime TimeStamp,
    string Ct1Name,
    string Ct2Name,
    string Ct3Name,
    decimal PhysicalCt1Value,
    decimal PhysicalCt2Value,
    decimal PhysicalCt3Value,
    decimal GeneratedValue,
    decimal GridValue,
    decimal DiversionAmount,
    Status Status,
    int TemperatureProbe1,
    int TemperatureProbe2,
    string Heater1Name,
    string Heater2Name,
    int ActiveHeater,
    int Priority,
    decimal TotalTransferred)
    : IMyEnergiProductWithCt;