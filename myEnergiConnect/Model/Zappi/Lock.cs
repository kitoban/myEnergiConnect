namespace myEnergiConnect.Model.Zappi;

[Flags]
public enum Lock
{
    LockedNow = 0,
    LockWhenPluggedIn = 1<<0,
    LockWhenUnplugged = 1<<1,
    ChargeWhenLocked = 1<<2,
    ChargeSessionAllowed = 1<<3,
    UnknownBit5 = 1<<4,
    UnknownBit6 = 1<<5,
    UnknownBit7 = 1<<6,
    UnknownBit8 = 1<<7,
}