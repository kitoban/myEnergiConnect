using myEnergiConnect.Model.External;

namespace MyEnergiConnect.Exceptions;

public class ItemNotFoundException : MyEnergiConnectException
{
    public int SerialNumber { get; init; }
    public MyEnergiProduct Product { get; init; }

    public ItemNotFoundException(int serialNumber, MyEnergiProduct product)
    {
        SerialNumber = serialNumber;
        Product = product;
    }
}