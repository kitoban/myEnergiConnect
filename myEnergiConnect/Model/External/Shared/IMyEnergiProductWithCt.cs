namespace myEnergiConnect.Model.External.Shared;

public interface IMyEnergiProductWithCt : IMyEnergiProduct
{
    string Ct1Name { get; init; }
    string Ct2Name { get; init; }
    string Ct3Name { get; init; }
    double PhysicalCt1Value { get; init; }
    double PhysicalCt2Value { get; init; }
    double PhysicalCt3Value { get; init; }
}