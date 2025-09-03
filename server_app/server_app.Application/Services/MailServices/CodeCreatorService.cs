using server_app.Application.Abstractions.EmailSend;
using server_app.Application.Extensions;

namespace server_app.Application.Services.MailServices;

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