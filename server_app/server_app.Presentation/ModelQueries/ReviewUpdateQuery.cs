using System.ComponentModel.DataAnnotations;

namespace server_app.Presentation.ModelQueries;

public class ReviewUpdateQuery
{
    [Required] public Guid Id { get; set; }

    [Required, StringLength(maximumLength: 500, MinimumLength = 10)]
    public string NewText { get; set; }

    [Required, Range(1, 10)] public int NewEstimation { get; set; }
}