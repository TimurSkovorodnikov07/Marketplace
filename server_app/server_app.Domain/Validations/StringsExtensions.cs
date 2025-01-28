using System.Text.RegularExpressions;

namespace server_app.Domain.Validations;

public static class StringsExtensions
{
    public static string LeaveOnlyTheNumbers(this string str) => Regex.Replace(str, @"\D", string.Empty);
}