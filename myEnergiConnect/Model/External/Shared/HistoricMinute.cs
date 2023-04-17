namespace MyEnergiConnect.Model.External.Shared;

public record HistoricMinute(
    DateTime DateTime,
    decimal GridFlow,
    decimal DiversionFlow,
    decimal? GenerationFlow,
    decimal? Ct1Flow,
    decimal? Ct2Flow,
    decimal? Ct3Flow);