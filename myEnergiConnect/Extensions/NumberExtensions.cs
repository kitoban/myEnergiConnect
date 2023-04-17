namespace myEnergiConnect.Extensions;

internal static class NumberExtensions
{
    public static double FromWattToKilowatt(this int value)
    {
        return value / 1000d;
    }
    
    public static double FromKiloWattToWatt(this double value)
    {
        return value * 1000d;
    }
    
    public static double FromWattSecondToToWattMinute(this int value)
    {
        return value / 60d;
    }
    
    public static double FromWattSecondToToKiloWattHours(this int value)
    {
        return value / 3_600_000d;
    }
}