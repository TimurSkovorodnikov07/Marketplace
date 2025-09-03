namespace server_app.Domain.Model.Dtos;

public class ReviewDto
{
    public Guid Id { get; set; }
    public string Text { get; set; }
    public int Estimation { get; set; }
    public Guid CategoryId;
    public Guid ReviewOwnerId;    
}