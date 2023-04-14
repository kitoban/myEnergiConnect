namespace MyEnergiConnect.Model.External.Shared;

public record HistoricMinute(
    DateTime DateTime,
    double GridFlow,
    double ZappiFlow,
    double? GenerationFlow,
    double? Ct1Flow,
    double? Ct2Flow,
    double? Ct3Flow);