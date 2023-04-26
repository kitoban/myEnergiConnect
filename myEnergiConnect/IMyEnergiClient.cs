using myEnergiConnect.Model.Enums;
using myEnergiConnect.Model.External.Eddi;
using MyEnergiConnect.Model.External.Shared;
using myEnergiConnect.Model.External.Zappi;

namespace MyEnergiConnect;

public interface IMyEnergiClient
{
    Task<ZappiSumary[]> GetZappiSummaries(PowerUnits powerUnit = PowerUnits.Watt, EnergyUnit energyUnit = EnergyUnit.KiloWattHour);
    Task<EddiSummary[]> GetEddiSummaries(PowerUnits powerUnit = PowerUnits.Watt, EnergyUnit energyUnit = EnergyUnit.KiloWattHour);
    
    /// <summary>
    /// Extracts a specifics days data. If the gateway product was not connected to the internet then the data during this time will be omitted.
    /// Units of power are represented by the minute as this is what the data is broken down into.  
    /// </summary>
    /// <param name="serialNo">Serial of the Zappi</param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <param name="unit">Unit the power is converted to, note that this will be as a minute representation to the power as the data is broken up into minute segments.</param>
    /// <returns></returns>
    Task<HistoricDay> GetZappiHistory(int serialNo, int year, int month, int day, PowerUnits unit = PowerUnits.Watt);
    
    
    /// <summary>
    /// Extracts a specifics days data. If the gateway product was not connected to the internet then the data during this time will be omitted.
    /// Units of power are represented by the minute as this is what the data is broken down into.  
    /// </summary>
    /// <param name="serialNo">Serial of the Eddi</param>
    /// <param name="year"></param>
    /// <param name="month"></param>
    /// <param name="day"></param>
    /// <param name="unit">Unit the power is converted to, note that this will be as a minute representation to the power as the data is broken up into minute segments.</param>
    /// <returns></returns>
    Task<HistoricDay> GetEddiHistory(int serialNo, int year, int month, int day, PowerUnits unit = PowerUnits.Watt);
}