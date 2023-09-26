using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogPostsController : ControllerBase
    {
        private readonly IBlogPostRepository _blogPostRepository;
        private readonly ICategoryRepository _categoryRepository;

        public BlogPostsController(IBlogPostRepository blogPostRepository, ICategoryRepository categoryRepository)
        {
            _blogPostRepository = blogPostRepository;
            _categoryRepository = categoryRepository;
        }

        [Authorize(Roles = "Writer")]
        [HttpPost]
        public async Task<IActionResult> CreateBlogPost([FromBody] AddBlogPostRequestDto request)
        {
            var blogPost = request.MapPropertiesTo<AddBlogPostRequestDto, BlogPost>();
            blogPost.Id = Guid.NewGuid();
            blogPost.Categories = new List<Category>();
            foreach (var categoryId in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(categoryId);

                if (existingCategory is not null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }

            var result = await _blogPostRepository.CreateAsync(blogPost);
            var dtoResponse = result.MapPropertiesTo<BlogPost, AddBlogPostResponseDto>();
            dtoResponse.Categories = result.Categories.MapPropertiesTo<Category, CategoryDto>();
            return Ok(dtoResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllBlogPosts()
        {
            var blogPosts = await _blogPostRepository.GetAllAsync();

            // Convert Domain model to DTO
            List<BlogPostDto> blogPostDtos = blogPosts.MapPropertiesTo<BlogPost, BlogPostDto>((post, dto) =>
            {
                dto.Categories = post.Categories.MapPropertiesTo<Category, CategoryDto>();
            });

            return Ok(blogPostDtos);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetBlogPostById([FromRoute] Guid id)
        {
            var blogPost = await _blogPostRepository.GetByIdAsync(id);

            if (blogPost is null)
            {
                return NotFound();
            }

            BlogPostDto blogPostDto = blogPost.MapPropertiesTo<BlogPost, BlogPostDto>((post, dto) =>
            {
                dto.Categories = post.Categories.MapPropertiesTo<Category, CategoryDto>();
            });

            return Ok(blogPostDto);
        }

        [Authorize(Roles = "Writer")]
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateBlogPost([FromRoute] Guid id, [FromBody] UpdateBlogPostRequestDto request)
        {
            var blogPost = request.MapPropertiesTo<UpdateBlogPostRequestDto, BlogPost>();
            blogPost.Id = id;
            blogPost.Categories = new List<Category>();

            foreach (var categoryGuid in request.Categories)
            {
                var existingCategory = await _categoryRepository.GetByIdAsync(categoryGuid);

                if (existingCategory != null)
                {
                    blogPost.Categories.Add(existingCategory);
                }
            }
            var result = await _blogPostRepository.UpdateAsync(blogPost);

            if (result is null)
            {
                return NotFound();
            }

            BlogPostDto blogPostDto = blogPost.MapPropertiesTo<BlogPost, BlogPostDto>((post, dto) =>
            {
                dto.Categories = post.Categories.MapPropertiesTo<Category, CategoryDto>();
            });

            return Ok(blogPostDto);
        }

        [Authorize(Roles = "Writer")]
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteBlogPost([FromRoute] Guid id)
        {
            var result = await _blogPostRepository.DeleteAsync(id);

            if (!result)
            {
                return NotFound();
            }

            return Ok();
        }

        [HttpGet]
        [Route("{urlHandle}")]
        public async Task<IActionResult> GetBlogPostByUrlHandle([FromRoute] string urlHandle)
        {
            var blogPost = await _blogPostRepository.GetByUrlHandleAsync(urlHandle);

            if (blogPost is null)
            {
                return NotFound();
            }

            BlogPostDto blogPostDto = blogPost.MapPropertiesTo<BlogPost, BlogPostDto>((post, dto) =>
            {
                dto.Categories = post.Categories.MapPropertiesTo<Category, CategoryDto>();
            });

            return Ok(blogPostDto);
        }

    }
}
