public class CodeService : ICodeCreator
{
    public string? Create(int length)
    {
        var list = new Random().NextIEnumerable(length, 0, 9);

        return list == null
            ? null 
            : string.Join("", list);
    }
}