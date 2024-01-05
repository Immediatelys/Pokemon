using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApplication1.Data;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : Controller
    {
        private readonly IPokemonRepository _pokemonRepository;
        private readonly IMapper _mapper;

        public PokemonController(IPokemonRepository pokemonRepository, IMapper mapper)
        {
            _pokemonRepository = pokemonRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        public IActionResult GetPokemons()
        {
            var pokemons = _pokemonRepository.GetPokemons();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public IActionResult GetPokemon(int pokeId) 
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var pokemon = _pokemonRepository.GetPokemon(pokeId);

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            return Ok(pokemon);

        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]

        public IActionResult PokemonRating(int pokeId) 
        {
            if (!_pokemonRepository.PokemonExists(pokeId))
                return NotFound();

            var rating = _pokemonRepository.GetPokemonRating(pokeId);

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);
            return Ok(rating);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreatePokemon([FromQuery] int ownerId, [FromQuery] int cateId, [FromBody] PokemonDto pokemonCreate)
        {
            if (pokemonCreate == null)
                return BadRequest(ModelState);

            var pokemons = _pokemonRepository.GetPokemons()
                .Where(p => p.Name.Trim().ToUpper() == pokemonCreate.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (pokemons != null)
            {
                ModelState.AddModelError("", "Pokemon is already exist");
                return StatusCode(422, ModelState);
            }

            var pokemonMap = _mapper.Map<Pokemon>(pokemonCreate);

            if (!_pokemonRepository.CreatePokemon(ownerId, cateId, pokemonMap))
            {
                ModelState.AddModelError("","Something wrong while saving");
                return StatusCode(500,ModelState);
            }

            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            return Ok("Create success");
        }

    }
}
