using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class CategoryController : Controller
    {
        public readonly ICategoryRepository _categoryRepository;
        public readonly IMapper _mapper;
        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetPokemons()
        {
            var categories = _mapper.Map<List<CategoryDto>>(_categoryRepository.GetCategories());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(categories);
        }


        [HttpGet("{cateId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int cateId)
        {
            if (!_categoryRepository.CategoryExists(cateId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_categoryRepository.GetCategory(cateId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(category);
        }

        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]

        public IActionResult GetPokemonsByCategory(int categoryId)
        {
            if(!_categoryRepository.CategoryExists(categoryId))
                return NotFound();

            var pokemon = _mapper.Map<List<PokemonDto>>(_categoryRepository.GetPokemonsByCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemon);
        }

        [HttpPost]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        //categoryCreate is new cate that we create in swager 

        public IActionResult CreateCategory([FromBody] CategoryDto categoryCreate)
        {
            if(categoryCreate == null)
            {
                return BadRequest(ModelState);
            }


            //check category is exist in database
            var category = _categoryRepository.GetCategories()
                .Where(c => c.Name.Trim().ToUpper() == categoryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if(category != null)
            {
                ModelState.AddModelError("", "Category is already exist");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(categoryCreate);

            if (!_categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create succesful");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory)
        {
            if (updatedCategory == null) 
            {
                return BadRequest(ModelState) ;
            }

            if(categoryId != updatedCategory.Id) 
            {
                return BadRequest(ModelState);
            }
           

            if(!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryMap = _mapper.Map<Category>(updatedCategory);

            if(!_categoryRepository.UpdateCategory(categoryMap))
            {
                ModelState.AddModelError("", "Something worng when updating");
                return StatusCode(500, ModelState);
            }

            return Ok("Update successful");
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if(!_categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryDelete = _categoryRepository.GetCategory(categoryId);

           if(!ModelState.IsValid)
           {
                return BadRequest(ModelState);  
           }

           if(!_categoryRepository.DeleteCategory(categoryDelete))
            {
                ModelState.AddModelError("", "Something wrong when delete");
                return StatusCode(500, ModelState); 
            }

            return Ok("delete succesfull");

        }

    }
}

