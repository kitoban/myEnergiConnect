using myEnergiConnect.Model.Enums;
using myEnergiConnect.Model.External.Eddi;
using MyEnergiConnect.Model.External.Shared;
using myEnergiConnect.Model.External.Zappi;

namespace MyEnergiConnect;

public interface IMyEnergiClient
{
    Task<ZappiSumary[]> GetZappiSummaries(PowerUnits powerUnit = PowerUnits.Watt, EnergyUnit energyUnit = EnergyUnit.KiloWattHour);
    Task<EddiSummary[]> GetEddiSummaries(PowerUnits powerUnit = PowerUnits.Watt, EnergyUnit energyUnit = EnergyUnit.KiloWattHour);
    Task<HistoricDay> GetZappiHistory(int serialNo, int year, int month, int day, EnergyUnit unit = EnergyUnit.WattSecond);
    Task<HistoricDay> GetEddiHistory(int serialNo, int year, int month, int day, EnergyUnit unit = EnergyUnit.WattSecond);
}