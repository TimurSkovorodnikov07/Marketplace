using System.ComponentModel.DataAnnotations;

[CreditCardAddQueryValidation]
public class CreditCardAddQuery
{
    [Required, MaxLength(20)] public string Number { get; set; }
    [Required, Range(0, int.MaxValue)] public decimal Many { get; set; } //Думая максимум int-а будет достаточно
    [Required] public string Type { get; set; }
}