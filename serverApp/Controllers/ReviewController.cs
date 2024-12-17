using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("/api/reviews")]
public class ReviewController(
    ReviewService service,
    CustomerService customerService,
    ProductCategoryService productCategoryService,
    IMapper mapper,
    ILogger<ReviewController> logger) : ControllerBase
{
    public const string GetReviewsMaxCount = "X-Max-Count";

    [HttpGet("{categoryId:guid}"), ValidationFilter, Authorize]
    public async Task<IActionResult> GetReview([Required] Guid categoryId)
    {
        var ownerId = GetCustomerId();

        if (ownerId == null)
            return Forbid();

        var review = await service.GetByOwnerAndCategoryIdentifiers(
            categoryId: categoryId,
            ownerid: (Guid)ownerId);

        return Ok(review);
    }

    [HttpGet, ValidationFilter]
    public async Task<IActionResult> GetReviews([FromQuery, Required] GetReviewsQuery query)
    {
        var result = await service.GetReviews(query, GetCustomerId());

        if (result == null)
            return NotFound("Category not found");

        Response.Headers.Append(GetReviewsMaxCount, result.MaxCount.ToString());
        return Ok(result.ReviewDtos);
    }

    [HttpPost, Authorize, ValidationFilter]
    public async Task<IActionResult> AddReview([FromForm, Required] AddReviewQuery query)
    {
        var reviewOwnerId = GetCustomerId();
        if (reviewOwnerId == null) return Forbid();

        var foundCategory = await productCategoryService.Get(query.CategoryId);
        if (foundCategory == null)
            return NotFound("Category not found");

        var isBought =
            await productCategoryService.IsBought(categoryId: query.CategoryId, buyerId: (Guid)reviewOwnerId);
        if (isBought == false)
            return Forbid();

        var foundOwner = await customerService.Get((Guid)reviewOwnerId);
        if (foundOwner == null)
            return NotFound("Category not found");

        var existsReview = await service.GetByOwnerAndCategoryIdentifiers(query.CategoryId, foundOwner.Id);
        if (existsReview != null)
            return BadRequest("You have review to this product category.");


        var newReview = ReviewEntity.Create(query.Text, query.Estimation, foundCategory, foundOwner);

        if (newReview == null)
            return BadRequest("New review not valid");

        await service.Add(newReview);
        return Ok();
    }

    [HttpPut, Authorize, ValidationFilter]
    public async Task<IActionResult> Update([FromForm, Required] ReviewUpdateQuery query)
    {
        var ownerId = GetCustomerId();
        if (ownerId == null) return Forbid();

        var result = await service.Update(query.Id, query.NewText, query.NewEstimation, (Guid)ownerId);
        
        return result.ActionResult;
    }

    [HttpDelete("{guid:guid}"), Authorize, ValidationFilter]
    public async Task<IActionResult> Remove(Guid guid)
    {
        var foundReview = await service.Get(guid);

        if (foundReview == null)
            return NotFound("Review not found");

        if (IsOwner(GetCustomerId(), foundReview) == false)
            return Forbid();

        var result = await service.Remove(foundReview);
        return result.ActionResult;
    }


    private Guid? GetCustomerId() => User.Claims.TryIsCustomer(out var guid) ? guid : null;

    private bool IsOwner(Guid? customerId, ReviewEntity? review) => (customerId != Guid.Empty && customerId != null)
                                                                    && review != null
                                                                    && customerId == review.ReviewOwnerId;
}