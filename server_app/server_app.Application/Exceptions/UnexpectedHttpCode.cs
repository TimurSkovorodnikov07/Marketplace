namespace server_app.Application.Exceptions;

public class UnexpectedHttpCode(string message) : Exception(message)
{
}