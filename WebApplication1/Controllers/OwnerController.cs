using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IOwnerRepository _ownerRepository;
        private readonly ICountryRepository _countryRepository;
        public OwnerController(IMapper mapper, IOwnerRepository ownerRepository, ICountryRepository countryRepository)
        {
            _mapper = mapper;
            _ownerRepository = ownerRepository;
            _countryRepository = countryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwners()
        {
            var owners = _mapper.Map<List<OwnerDto>>(_ownerRepository.GetOwners());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }

        

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        public IActionResult GetOwner(int ownerId)
        {
            if (!_ownerRepository.OwnwersExist(ownerId))
            {
                return NotFound();
            }

            var owners = _mapper.Map<OwnerDto> (_ownerRepository.GetOwner(ownerId));
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(owners);
        }



        [HttpGet("{ownerId}/pokemon")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]


        public IActionResult GetPokemonByOwner(int ownerId)
        {
            if (!_ownerRepository.OwnwersExist(ownerId)) { return NotFound(); }

            var pokemons = _mapper.Map<List<PokemonDto>>(_ownerRepository.GetPokemonByOwner(ownerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            return Ok(pokemons);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateOwner([FromQuery] int countryId, [FromBody]OwnerDto ownerCreate )
        {
            if(ownerCreate == null)
            {
                return BadRequest(ModelState);
            }

            var owner = _ownerRepository.GetOwners().Where(o => o.LastName == ownerCreate.LastName).FirstOrDefault();

            if(owner != null)
            {
                ModelState.AddModelError("", "Owner is already exist");
                return StatusCode(422, ModelState);
            }

            if(!ModelState.IsValid)
            { return BadRequest(ModelState); }

            var ownerMap = _mapper.Map<Owner>(ownerCreate);
            ownerMap.Country = _countryRepository.GetCountry(countryId);

            if (!_ownerRepository.CreateOwner(ownerMap))
            {
                ModelState.AddModelError("", "Some thing wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Create succesfull");

        }

    }
}
