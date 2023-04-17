namespace myEnergiConnect.Model.External.Shared;

public interface IMyEnergiProductWithCt : IMyEnergiProduct
{
    string Ct1Name { get; init; }
    string Ct2Name { get; init; }
    string Ct3Name { get; init; }
    decimal PhysicalCt1Value { get; init; }
    decimal PhysicalCt2Value { get; init; }
    decimal PhysicalCt3Value { get; init; }
}