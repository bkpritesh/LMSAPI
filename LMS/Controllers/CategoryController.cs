using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Data.Services;
using Data.Repositary;
using Model;
using Microsoft.AspNetCore.Authorization;

namespace LMS.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        } 

      

        [HttpGet ]
        public async Task<IActionResult> Get()
        {
            var categories = await _categoryService.GetCategories();
            return Ok(categories);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryByID(string id)
        {
            var category = await _categoryService.GetCategoryByID(id);

            if (id != null)
            {
                category.CategoryId = id;
            }
            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }



        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] AddCategoriesOnly categories)
        {
            var result = await _categoryService.CreateCategory(categories);
            return Ok(result);
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(string id, [FromBody] Category category)
        {
            if (id != category.CategoryId)
            {
                return BadRequest("The category ID in the URL doesn't match the one in the request body.");
            }

            var updatedCategory = await _categoryService.UpdateCategory(category);

            if (updatedCategory == null)
            {
                return NotFound($"No category found with ID {id}");
            }

            return Ok(updatedCategory);
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(string id)
        {
            await _categoryService.DeleteCategory(id);

            return NoContent();
        }





        [HttpGet]
        [Route("Course/{CategoryId}")]
        public async Task<IActionResult> GetCourseByCategoryID(string CategoryId)
        {
            var Category = await _categoryService.GetCourseByCategoryID(CategoryId);

            if (Category == null)
            {
                return NotFound(false);
            }

            return Ok(Category);
        }

    }
}
