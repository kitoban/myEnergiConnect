using System.Runtime.Serialization;

namespace MyEnergiConnect.Exceptions;

public abstract class MyEnergiConnectException : Exception
{
    public MyEnergiConnectException()
    {
    }

    protected MyEnergiConnectException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public MyEnergiConnectException(string? message) : base(message)
    {
    }

    public MyEnergiConnectException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}