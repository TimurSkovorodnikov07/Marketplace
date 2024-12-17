public class RatingFromCustomerEntity : Entity
{
    public Guid CustomerId { get; set; }
    public CustomerEntity Customer { get; set; }

    public int Rating { get; set; }
    public RatingEntity CommonRating { get; set; }
    public Guid CommonRatingId { get; set; }


    public static RatingFromCustomerEntity? Create(Guid customerId, RatingEntity commonRating)
    {
        var newRatting = new RatingFromCustomerEntity
        {
            CustomerId = customerId,
            CommonRating = commonRating,
            Rating = 0
        };

        return RatingFromCustomerValidator.IsValid(newRatting) ? newRatting : null;
    }
}