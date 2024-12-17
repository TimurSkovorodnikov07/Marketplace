using System.ComponentModel.DataAnnotations;

public class CategoryBuyDto
{
    [Required] public Guid PurchasedCategoryId { get; set; }
    [Required, Range(1, int.MaxValue)] public int NumberOfPurchases { get; set; } = 1;
}