public class Tokens(string access, string refresh)
{
    public string AccessToken { get; set; } = access;
    public string RefreshToken { get; set; } = refresh;
}