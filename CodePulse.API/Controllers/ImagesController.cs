using CodePulse.API.Models.Domain;
using CodePulse.API.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IImageRepository _imageRepository;

        public ImagesController   (IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }
        
        [HttpPost]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile file, [FromForm] string fileName, [FromForm] string title)
        {
            ValidateFileUpload(file);

            if (ModelState.IsValid == false)
            {
                return BadRequest(ModelState);
            }

            BlogImage blogImage = new BlogImage
            {                
                FileExtension = Path.GetExtension(file.FileName).ToLower(),
                FileName = fileName,
                Title = title,
                DateCreated = DateTime.Now
            };

            var result = await _imageRepository.Upload(file, blogImage);
            return Ok(result);
        }

        private void ValidateFileUpload(IFormFile file)
        {
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };

            if (allowedExtensions.Contains(Path.GetExtension(file.FileName).ToLowerInvariant()) == false)
            {
                ModelState.AddModelError("File", "Invalid file extension.");
            }

            if (file.Length > 10485760)
            {
                ModelState            .AddModelError("File", "The file is too large. Please upload a file less than 10MB.");
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllImages()
        {
            var images = await _imageRepository.GetAllAsync();
            return Ok(images);
        }
    }
}
