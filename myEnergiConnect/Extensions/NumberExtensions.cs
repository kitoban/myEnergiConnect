namespace myEnergiConnect.Extensions;

internal static class NumberExtensions
{
    public static decimal FromWattToKilowatt(this int value)
    {
        return value / 1000m;
    }
    
    public static decimal FromKiloWattHourToWattMinutes(this decimal value)
    {
        return value * 60_000m;
    }
    
    public static decimal FromKiloWattHourToWattSeconds(this decimal value)
    {
        return value * 3_600_000m;
    }
    
    public static decimal FromWattSecondToToWattMinute(this int value)
    {
        return value / 60m;
    }
    
    public static decimal FromWattSecondToToKiloWattHours(this int value)
    {
        return value / 3_600_000m;
    }
}