using System.ComponentModel.DataAnnotations;

public class GetCategoriesQuery : BaseGetQuery
{
    public string? Search { get; set; } = null;

    [Range(0, int.MaxValue)] public int PriceNoMoreThenOrEqual { get; set; } = 0;
    // TODO: Also need to add filters by tags 

    [Required]public Guid? SellerId { get; set; } = null;
}