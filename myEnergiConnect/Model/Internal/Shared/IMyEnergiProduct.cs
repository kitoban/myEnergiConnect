﻿namespace myEnergiConnect.Model.Internal.Shared;

public interface IMyEnergiProduct
{
    int SerialNumber { get; init; }
    string Ct1Name { get; init; }
    string Ct2Name { get; init; }
    string Ct3Name { get; init; }
    int PhysicalCt1ValueWatts { get; init; }
    int PhysicalCt2ValueWatts { get; init; }
    int PhysicalCt3ValueWatts { get; init; }
}