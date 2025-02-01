using System.ComponentModel.DataAnnotations;

namespace server_app.Presentation.ModelQueries;

public class GetReviewsQuery : BaseGetQuery
{
    [Required] public Guid CategoryId { get; set; }
}