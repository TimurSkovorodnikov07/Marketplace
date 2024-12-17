using System.ComponentModel.DataAnnotations;

public class AddReviewQuery
{
    [Required, StringLength(maximumLength: 500, MinimumLength = 10)]
    public string Text { get; set; }

    [Required, Range(1, 10)] public int Estimation { get; set; }

    [Required] public Guid CategoryId { get; set; }
}