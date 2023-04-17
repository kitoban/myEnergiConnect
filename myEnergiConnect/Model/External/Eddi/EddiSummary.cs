using myEnergiConnect.Model.Enums;
using myEnergiConnect.Model.External.Shared;

namespace myEnergiConnect.Model.External.Eddi;

public record EddiSummary(
    int SerialNumber,
    string Ct1Name,
    string Ct2Name,
    string Ct3Name,
    double PhysicalCt1Value,
    double PhysicalCt2Value,
    double PhysicalCt3Value,
    double GeneratedValue,
    double GridValue,
    double DiversionAmount,
    Status Status,
    int TemperatureProbe1,
    int TemperatureProbe2,
    string Heater1Name,
    string Heater2Name,
    int ActiveHeater,
    int Priority,
    double TotalTransferred)
    : IMyEnergiProductWithCt;