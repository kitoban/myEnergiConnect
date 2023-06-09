﻿using System.Runtime.Serialization;

namespace myEnergiConnect.Model.Enums;

public enum PlugStatus
{
    [EnumMember(Value = "A")]
    EvDisconnected,
    
    [EnumMember(Value = "B1")]
    EvConnected,
    
    [EnumMember(Value = "B2")]
    WaitingForEv,
    
    [EnumMember(Value = "C1")]
    ReadyToCharge,
    
    [EnumMember(Value = "C2")]
    Charging,
    
    [EnumMember(Value = "F")]
    Fault
}