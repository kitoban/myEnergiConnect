namespace MyEnergiConnect.Model.External.Shared;

public record HistoricDay(
    string Ct1Name,
    string Ct2Name,
    string Ct3Name,
    EnergyUnit EnergyUnit,
    HistoricMinute[] MinuteValue);