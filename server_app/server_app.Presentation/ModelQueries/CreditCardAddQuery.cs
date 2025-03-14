using System.ComponentModel.DataAnnotations;

namespace server_app.Presentation.ModelQueries;

public class CreditCardAddQuery
{
    [Required, MaxLength(20)] public string Number { get; set; }
    [Required, Range(0, int.MaxValue)] public decimal Many { get; set; }
    [Required] public string Type { get; set; }
}