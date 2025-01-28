using server_app.Domain.Entities.Users.Customer;
using server_app.Domain.Validations;

namespace server_app.Domain.Entities.Users.CreditCard;

public class CreditCardEntity : Entity
{
    public string NumberHash { get; set; }
    public decimal Many { get; set; }
    public CreditCardType Type { get; set; }
    
    public CustomerEntity Owner { get; set; }
    public Guid OwnerId { get; set; }

    public static CreditCardEntity? Create(string numberHash, CustomerEntity owner, CreditCardType type, decimal many)
    {
        var newCard = new CreditCardEntity
        {
            NumberHash = numberHash,
            Type = type,
            Owner = owner,
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