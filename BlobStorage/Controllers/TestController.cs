using BlobStorage.Dtos;
using BlobStorage.Services;
using Microsoft.AspNetCore.Mvc;

namespace BlobStorage.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    private readonly BlobStorageService _blobStorageService;

    public TestController(BlobStorageService blobStorageService)
    {
        _blobStorageService = blobStorageService;
    }

    [HttpPost]
    public async Task Upload(IFormFile picture)
    {
        var newFileName = Guid.NewGuid().ToString() + Path.GetExtension(picture.FileName);

        await _blobStorageService.UploadAsync(picture.OpenReadStream(), newFileName);
    }
    
    [HttpPost("download")]
    public async Task<IActionResult> Download(FileNameRequest request)
    {
        var stream = await _blobStorageService.DownloadAsync(request);

        return File(stream, "application/octet-stream", request.FileName);
    }

    [HttpPost("delete")]
    public async Task Delete(FileNameRequest request)
    {
        await _blobStorageService.DeleteAsync(request);
    }
}