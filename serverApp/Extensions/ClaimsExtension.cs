using System.Security.Claims;

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
    private static bool IsSeller(IEnumerable<Claim> claims) => GetUserType(claims) == "seller";

    private static bool TryGet(IEnumerable<Claim> claims, bool isSeller, string selectType, out string? result)
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


    public static bool TryGetSellerIdValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, true, JwtService.UserIdClaimType, out result);

    public static bool TryGetSellerNameValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, true, JwtService.UserNameClaimType, out result);

    public static bool TryGetSellerEmailValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, true, JwtService.UserEmailClaimType, out result);


    public static bool TryGetCustomerIdValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, false, JwtService.UserIdClaimType, out result);

    public static bool TryGetCustomerNameValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, false, JwtService.UserNameClaimType, out result);

    public static bool TryGetCustomerEmailValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, false, JwtService.UserEmailClaimType, out result);


    public static bool TryIsCustomer(this IEnumerable<Claim> claims, out Guid? guid)
    {
        if (TryGetCustomerIdValue(claims, out string? guidString)
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
        if (TryGetSellerIdValue(claims, out string? sellerGuidString)
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