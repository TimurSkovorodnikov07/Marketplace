using System.ComponentModel.DataAnnotations;

public class GetCategoriesQuery
{
    [Required] public int From { get; set; }
    [Required] public int To { get; set; }
    public  string? Search { get; set; }
    public int PriceNoMoreThenOrEqual { get; set; }
    
    
    public Guid? SellerId { get; set; }
}