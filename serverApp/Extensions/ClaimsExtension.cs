using System.Security.Claims;

public static class ClaimsExtension
{
    private static string? GetValue(IEnumerable<Claim> claims, string selectType) =>
        claims.FirstOrDefault(c => c.Type == selectType)?.Value;

    private static bool IsSeller(IEnumerable<Claim> claims) => GetUserType(claims) == "seller";

    private static bool TryGet(IEnumerable<Claim> claims, bool isSeller, string selectType, out string? result)
    {
        if (IsSeller(claims) == isSeller)
        {
            result = GetValue(claims, selectType);
            return true;
        }

        result = null;
        return false;
    }

    public static string? GetUserType(this IEnumerable<Claim> claims) => GetValue(claims, "userType");


    public static bool TryGetSellerIdValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, true, "userId", out result);

    public static bool TryGetSellerNameValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, true, "userName", out result);

    public static bool TryGetSellerEmailValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, true, "userEmail", out result);


    public static bool TryGetCustomerIdValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, false, "userId", out result);

    public static bool TryGetCustomerNameValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, false, "userName", out result);

    public static bool TryGetCustomerEmailValue(this IEnumerable<Claim> claims, out string? result) =>
        TryGet(claims, false, "userEmail", out result);

    
    
    
    public static bool TryIsCustomer(this IEnumerable<Claim> claims, out Guid? guid)
    {
        if (TryGetCustomerIdValue(claims, out string? guidString)
            && Guid.TryParse(guidString, out Guid buyerGuid))
        {
            guid = buyerGuid;
            return true;
        }

        guid = null;
        return false;
    }
    public static bool TryIsSeller(this IEnumerable<Claim> claims, out Guid? guid)
    {
        if (TryGetSellerIdValue(claims, out string? sellerGuidString)
            && Guid.TryParse(sellerGuidString, out Guid sellerGuid))
        {
            guid = sellerGuid;
            return true;
        }

        guid = null;
        return false;
    }


    public static string? GetUserIdValue(this IEnumerable<Claim> claims) => GetValue(claims, "userId");
}