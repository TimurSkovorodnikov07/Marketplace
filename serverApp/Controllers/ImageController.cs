using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

[Route("/api/images")]
[ApiController]
public class ImageController(ImagesService imagesService) : ControllerBase
{
    [HttpGet("{guid:guid}"), ValidationFilter]
    public async Task<IActionResult> Get([Required] Guid guid)
    {
        var foundImage = await imagesService.Get(guid);

        if (foundImage == null || System.IO.File.Exists(foundImage.Path) == false)
            return NotFound("Image not found");


        var bytes = await System.IO.File.ReadAllBytesAsync(foundImage.Path);
        return File(bytes, foundImage.MimeType);
    }
}