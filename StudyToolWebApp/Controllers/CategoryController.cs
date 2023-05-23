using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using StudyToolWebApp.Dto;
using StudyToolWebApp.Models;
using StudyToolWebApp.Repository.ClassRepository;
using StudyToolWebApp.Repository.InterfaceRepository;

namespace StudyToolWebApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository repository) 
        { 
            _categoryRepository = repository;
        }

        [HttpGet]
        public IActionResult GetCategories()
        {
            var categories = _categoryRepository.GetCategories();

            ICollection<CategoryDto> categoriesDto = new List<CategoryDto>();

            foreach(Category c in categories)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    Color = c.Color
                });
            }
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categoriesDto);
        }

        [HttpGet("{categoryId}")]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var category = _categoryRepository.GetCategory(categoryId);
            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                Color = category.Color
            };

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(categoryDto);
        }

        [HttpGet("card/{categoryId}")]
        public IActionResult GetCardsByCategoryId(int categoryId)
        {
            var cards = _categoryRepository.GetCardsByCategory(categoryId);

            ICollection<CardDto> cardsDto = new List<CardDto>();

            foreach (Card c in cards)
            {
                cardsDto.Add(new CardDto
                {
                    Id = c.Id,
                    Term = c.Term,
                    Description = c.Description,
                    Important = c.Important
                });
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(cardsDto);
        }
    }
}
