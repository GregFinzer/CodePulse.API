using CodePulse.API.Models.Domain;
using CodePulse.API.Models.DTO;
using CodePulse.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CodePulse.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [Authorize(Roles = "Writer")]
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoryRequestDto request)
        {
            var category = request.MapPropertiesTo<AddCategoryRequestDto, Category>();
            var result = await _categoryRepository.CreateAsync(category);
            var dtoResponse = result.MapPropertiesTo<Category, CategoryDto>();
            return Ok(dtoResponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await _categoryRepository.GetAllAsync();
            var dtoResponse = categories.MapPropertiesTo<Category, CategoryDto>();
            return Ok(dtoResponse);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute] Guid id)
        {
            var category = await _categoryRepository.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            var dtoResponse = category.MapPropertiesTo<Category, CategoryDto>();
            return Ok(dtoResponse);
        }

        [Authorize(Roles = "Writer")]
        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id,
            [FromBody] UpdateCategoryRequestDto request)
        {
            var category = request.MapPropertiesTo<UpdateCategoryRequestDto, Category>();
            category.Id = id;
            var result = await _categoryRepository.UpdateAsync(category);

            if (result == null)
            {
                return NotFound();
            }

            var dtoResponse = result.MapPropertiesTo<Category, CategoryDto>();
            return Ok(dtoResponse);
        }

        [Authorize(Roles = "Writer")]
        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id)
        {
            var result = await _categoryRepository.DeleteAsync(id);
            if (!result)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
