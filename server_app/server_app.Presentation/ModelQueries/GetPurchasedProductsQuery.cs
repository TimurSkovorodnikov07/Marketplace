using System.ComponentModel.DataAnnotations;

namespace server_app.Presentation.ModelQueries;

public class GetPurchasedProductsQuery
{
    [Required, Range(0, int.MaxValue)] public int From { get; set; }
    [Required, Range(0, int.MaxValue)] public int To { get; set; }
}