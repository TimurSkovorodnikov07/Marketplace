using server_app.Domain.Model.Dtos;

namespace server_app.Domain.Model;

public class ReviewsModel
{
    public IEnumerable<ReviewDto> ReviewDtos { get; set; }
    public int MaxCount { get; set; }   
}