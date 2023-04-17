using myEnergiConnect.Model.Enums;

namespace myEnergiConnect.Model.External.Zappi;

public record ZappiSumary(
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
    int Priority,
    Status UnitStatus,
    double ChargeAdded,
    Enable BoostSmart,
    Lock LockStatus,
    PlugStatus PlugStatus,
    Mode Mode,
    int MinimumGreenLevel,
    int SmartBoostStartTimeHour,
    double SmartBoostKWhToAdd,
    int SmartBoostStartTimeMinute,
    int BoostHour,
    double BoostValue,
    int BoostMinute);