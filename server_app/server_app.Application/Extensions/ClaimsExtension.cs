using System.Security.Claims;
using server_app.Application.Services;

namespace server_app.Application.Extensions;

public static class ClaimsExtension
{
    private static string? GetValue(IEnumerable<Claim> claims, string selectType)
    {
        if (claims.Count() > 0 && string.IsNullOrEmpty(selectType) == false)
        {
            var first = claims.First(c => c.Type == selectType);

            if (first is null)
                throw new KeyNotFoundException("Selected type not found");

            return first.Value;
        }

        return null;
    }

    public static string? GetUserType(this IEnumerable<Claim> claims) => GetValue(claims, JwtService.UserTypeClaimType);

    private static bool IsSeller(IEnumerable<Claim> claims) =>
        string.Equals(GetUserType(claims), "seller", StringComparison.OrdinalIgnoreCase);

    public static bool TryGet(IEnumerable<Claim> claims, string selectType, bool isSeller, out string? result)
    {
        if (IsSeller(claims) == isSeller)
        {
            result = GetValue(claims, selectType);

            if (string.IsNullOrEmpty(result) == false)
                return true;
        }

        result = null;
        return false;
    }


    public static bool TryIsCustomer(this IEnumerable<Claim> claims, out Guid? guid)
    {
        if (TryGet(claims, JwtService.UserIdClaimType, false, out string? guidString)
            && Guid.TryParse(guidString, out Guid customerGuid))
        {
            guid = customerGuid;
            return true;
        }

        guid = null;
        return false;
    }

    public static bool TryIsSeller(this IEnumerable<Claim> claims, out Guid? guid)
    {
        if (TryGet(claims, JwtService.UserIdClaimType, true, out string? sellerGuidString)
            && Guid.TryParse(sellerGuidString, out var sellerGuid))
        {
            guid = sellerGuid;
            return true;
        }

        guid = null;
        return false;
    }


    public static string? GetUserIdValue(this IEnumerable<Claim> claims) =>
        GetValue(claims, JwtService.UserIdClaimType);
}