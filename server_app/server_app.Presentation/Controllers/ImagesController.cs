using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using server_app.Application.Repositories;
using server_app.Infrastructure.Repositories;
using server_app.Infrastructure.Repositories.ProductCategories;
using server_app.Presentation.Filters;

namespace server_app.Presentation.Controllers;

[Route("/api/images")]
[ApiController]
public class ImagesController(IImageRepository imageRepository, ILogger<ImagesController> logger) : ControllerBase
{
    [HttpGet("{guid:guid}"), ValidationFilter]
    public async Task<IActionResult> Get([Required] Guid guid)
    {
        var image = await imageRepository.GetById(guid);

        if (image is null)
            return NotFound("Image not found");

        return File(image.ImageData, image.MimeType);
    }

    [HttpGet("by-category-id/{categoryId:guid}"), ValidationFilter]
    public async Task<IActionResult> GetImages([Required] Guid categoryId)
    {
        var images = await imageRepository.GetByProductCategoryId(categoryId);

        if (images.Any())
            logger.LogCritical("There aren no images in the category");

        return Ok(images);
    }
}