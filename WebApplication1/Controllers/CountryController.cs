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
    public class CountryController : Controller
    {
        private readonly ICountryRepository _countryRepository;
        private readonly IMapper _mapper;
        public CountryController(ICountryRepository countryRepository, IMapper mapper)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]

        public IActionResult GetCountries()
        {
            var countries = _mapper.Map<List<CountryDto>>(_countryRepository.GetCountries());
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(countries);
        }

        [HttpGet("{countryID}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]

        public IActionResult GetCountry(int countryID)
        {
            if(!_countryRepository.CountryExists(countryID))
            {
                return NotFound();
            }

            var country = _mapper.Map<CountryDto>(_countryRepository.GetCountry(countryID));

            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(country);
        }

        [HttpGet("/owners/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]

        public IActionResult GetCountryByOwner(int ownerId)
        {
           
            var country = _mapper.Map< CountryDto> (_countryRepository.GetCountryByOwner(ownerId));
            
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);

            }
            return Ok(country);
            
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateCountry([FromBody] CountryDto countryCreate)
        {
            if(countryCreate == null)
            {
                return BadRequest(ModelState);  
            }

            var country = _countryRepository.GetCountries()
                .Where(c => c.Name.Trim().ToUpper() ==  countryCreate.Name.TrimEnd().ToUpper()).FirstOrDefault(); 

            if(country != null) 
            {
                ModelState.AddModelError("", "Country is already exist ");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var countryMap = _mapper.Map<Country>(countryCreate);

            if(!_countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", "Something wrong when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("create succesful");

        }

        [HttpDelete("{countryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCountry(int countryId)
        {
            if(!_countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var countryDelete = _countryRepository.GetCountry(countryId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);  
            }
            
            if(!_countryRepository.DeleteCountry(countryDelete))
            {
                ModelState.AddModelError("", "Delete wrong");
                return StatusCode(500, ModelState);
            }

            return Ok("Delete successfull");
        }


    }
}
