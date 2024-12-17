using System.ComponentModel.DataAnnotations;

public class GetReviewsQuery : BaseGetQuery
{
    [Required] public Guid CategoryId { get; set; }
}