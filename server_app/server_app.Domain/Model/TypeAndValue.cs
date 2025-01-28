namespace server_app.Domain.Model;

public struct TypeAndValuePair(string type, string value)
{
    public string Type { get; set; } = type;
    public string Value { get; set; } = value;
}