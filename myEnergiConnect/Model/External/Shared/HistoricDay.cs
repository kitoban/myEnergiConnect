using myEnergiConnect.Model.Enums;

namespace MyEnergiConnect.Model.External.Shared;

public record HistoricDay(
    DateTime Date,
    string Ct1Name,
    string Ct2Name,
    string Ct3Name,
    PowerUnits PowerUnit,
    HistoricMinute[] MinuteValue);