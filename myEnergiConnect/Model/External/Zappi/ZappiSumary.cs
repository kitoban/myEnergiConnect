using myEnergiConnect.Model.Enums;

namespace myEnergiConnect.Model.External.Zappi;

public record ZappiSumary(
    int SerialNumber,
    DateTime TimeStamp,
    string Ct1Name,
    string Ct2Name,
    string Ct3Name,
    decimal PhysicalCt1Value, //decimal
    decimal PhysicalCt2Value,
    decimal PhysicalCt3Value,
    decimal GeneratedValue,
    decimal GridValue,
    decimal DiversionAmount,
    int Priority,
    Status UnitStatus,
    decimal ChargeAdded,
    Enable BoostManual,
    Enable BoostTimed,
    Enable BoostSmart,
    Lock LockStatus,
    PlugStatus PlugStatus,
    Mode Mode,
    int MinimumGreenLevel,
    int SmartBoostStartTimeHour,
    decimal SmartBoostKWhToAdd,
    int SmartBoostStartTimeMinute,
    int BoostHour,
    decimal BoostValue,
    int BoostMinute);