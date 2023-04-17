using MyEnergiConnect.Model.External.Shared;
using MyEnergiConnect.Model.Internal.Eddi;
using MyEnergiConnect.Model.Internal.Zappi;

namespace MyEnergiConnect;

public interface IMyEnergiClient
{
    Task<AllZappiSummary> GetZappiSummaries();
    Task<AllEddiSummary> GetEddiSummaries();
    Task<HistoricDay> GetZappiHistory(int serialNo, int year, int month, int day, EnergyUnit energyUnit = EnergyUnit.WattSecond);
    Task<HistoricDay> GetEddiHistory(int serialNo, int year, int month, int day, EnergyUnit energyUnit = EnergyUnit.WattSecond);
}