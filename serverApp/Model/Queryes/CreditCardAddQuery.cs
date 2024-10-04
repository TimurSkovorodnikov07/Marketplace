using System.ComponentModel.DataAnnotations;

public class CreditCardAddQuery
{
    [Required, MaxLength(20)] public string Number { get; set; }
    [Required, Range(0, int.MaxValue)] public decimal Many { get; set; } //Думая максимум int-а будет достаточно
    [Required] public CreditCardType Type { get; set; }
}