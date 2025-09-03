namespace server_app.Domain.Model.Dtos;

public class GetReviewsDto
{
    public int From { get; set; }
    public int To { get; set; }
    public Guid CategoryId { get; set; }
}