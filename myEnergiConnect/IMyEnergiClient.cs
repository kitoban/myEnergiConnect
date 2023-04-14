using MyEnergiConnect.Model.External.Shared;
using MyEnergiConnect.Model.Internal.Eddi;
using MyEnergiConnect.Model.Internal.Shared;
using MyEnergiConnect.Model.Internal.Zappi;

namespace MyEnergiConnect;

public interface IMyEnergiClient
{
    Task<AllZappiSummary> GetZappiSummaries();
    Task<AllEddiSummary> GetEddiSummaries();
    Task<HistoricDay> GetZappiHistory(int serialNo, int year, int month, int day, FlowUnit flowUnit = FlowUnit.Joules);
    Task<MinuteHistory[]> GetEddiHistory(int serialNo, int year, int month, int day);
}