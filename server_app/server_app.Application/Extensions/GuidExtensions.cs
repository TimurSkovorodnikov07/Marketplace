namespace server_app.Application.Extensions;

public static class GuidExtensions
{
    public static bool Equals(this Guid guid, string str)
    {
        return guid.ToString() == str;
    }

    public static bool NotEquals(this Guid guid, string str)
    {
        return guid.ToString() != str;
    }
}