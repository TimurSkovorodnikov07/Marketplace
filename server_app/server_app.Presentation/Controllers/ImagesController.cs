using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using server_app.Application.Services.EntitiesServices.Interfaces;
using server_app.Application.Services.FileServices;
using server_app.Presentation.Filters;

namespace server_app.Presentation.Controllers;

[Route("/api/images")]
[ApiController]
public class ImagesController(ImageMongoDbService imageMongoDbService) : ControllerBase
{
    [HttpGet("{guid:guid}"), ValidationFilter]
    public async Task<IActionResult> Get([Required] Guid guid)
    {
        var image = await imageMongoDbService.GetImage(guid);

        if (image is null)
            return NotFound("Image not found");

        return File(image.ImageData, image.MimeType);
    }
}