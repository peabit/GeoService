namespace GeoService.API.Exceptions;

public class ServiceException : Exception
{
    public ServiceException() : base("Internal service exception. Try later") { }
}