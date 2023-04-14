using myEnergiConnect.Model.Shared;

namespace myEnergiConnect;

public interface IClient
{
    Task<Model.Zappi.AllZappiSummary> GetZappiSummary();
    Task<Model.Eddi.AllEddiSummary> GetEddiSummary();
    Task<MinuteHistory[]> GetZappiHistory(int serialNo, int year, int month, int day);
    Task<MinuteHistory[]> GetEddiHistory(int serialNo, int year, int month, int day);
}