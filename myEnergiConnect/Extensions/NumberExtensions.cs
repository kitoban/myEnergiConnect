namespace myEnergiConnect.Extensions;

internal static class NumberExtensions
{
    public static decimal FromKiloWattHourToWattMinutes(this decimal value)
    {
        return value * 60_000m;
    }
    
    public static decimal FromKiloWattHourToWattSeconds(this decimal value)
    {
        return value * 3_600_000m;
    }
}