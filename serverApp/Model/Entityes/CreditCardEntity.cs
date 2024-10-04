public class CreditCardEntity : Entity
{
    public string Number { get; set; }
    public decimal Many { get; set; }
    public CreditCardType Type { get; set; }
    
    public Guid OwnerId { get; set; }

    public static CreditCardEntity? Create(string cardNumber, Guid ownerGuid, CreditCardType type, decimal many)
    {
        var newCard = new CreditCardEntity
        {
            Number = cardNumber,
            Type = type,
            OwnerId = ownerGuid,
            Many = many
        };
        return CreditCardValidator.IsValid(newCard) ? newCard : null;
    }
}

public enum CreditCardType
{
    MasterCard,
    VisaCard
}