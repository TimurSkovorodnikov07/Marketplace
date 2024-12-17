public class 
    RatingEntity
{
    public Guid ProductCategoryId { get; set; }
    public ProductCategoryEntity ProductCategory { get; set; }
    
    public int TotalRating { get; set; }
    public List<RatingFromCustomerEntity> RattingFromCustomers { get; set; }

    public static RatingEntity? Create(Guid categoryId)
    {
        var rating = new RatingEntity
        {
            ProductCategoryId = categoryId,
            TotalRating = 0
        };

        return RatingValidator.IsValid(rating) ? rating : null;
    }
}