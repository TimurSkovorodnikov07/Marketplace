using System.ComponentModel.DataAnnotations;

namespace server_app.Domain.Model.Queries;

public class GetReviewsQuery : BaseGetQuery
{
    [Required] public Guid CategoryId { get; set; }
}