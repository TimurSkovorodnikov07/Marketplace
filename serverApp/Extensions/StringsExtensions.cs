using System.Text.RegularExpressions;

public static class StringsExtensions
{
    public static string LeaveOnlyTheNumbers(this string str) => Regex.Replace(str, @"\D", string.Empty);
}