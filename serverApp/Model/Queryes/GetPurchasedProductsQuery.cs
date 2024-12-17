using System.ComponentModel.DataAnnotations;

public class GetPurchasedProductsQuery
{
    [Required, Range(0, int.MaxValue)] public int From { get; set; }
    [Required, Range(0, int.MaxValue)] public int To { get; set; }
}