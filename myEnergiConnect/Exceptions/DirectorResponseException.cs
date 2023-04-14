namespace MyEnergiConnect.Exceptions;

public class DirectorResponseException : MyEnergiConnectException
{
    public int StatusCode { get; }

    public DirectorResponseException(int statusCode, string? message) : base(message)
    {
        StatusCode = statusCode;
    }
}