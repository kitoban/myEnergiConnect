namespace myEnergiConnect.Excepetions;

public class DirectorResponseException : Exception
{
    public int StatusCode { get; }

    public DirectorResponseException(int statusCode, string? message) : base(message)
    {
        StatusCode = statusCode;
    }
}