using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Dto;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using WebApplication1.Repository;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewerController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewerController(IMapper mapper , IReviewerRepository reviewerRepository)
        {
            _mapper = mapper;
            _reviewerRepository = reviewerRepository;   
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]

        public IActionResult GetReviewers()
        {
            var reviewers = _mapper.Map<List<ReviewerDto>> (_reviewerRepository.GetReviewers()); 
            if(!ModelState.IsValid) 
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Reviewer>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewer(int reviewerId)
        {
            var reviewer = _mapper.Map<ReviewerDto>(_reviewerRepository.GetReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Review>))]
        [ProducesResponseType(400)]
        public IActionResult GetReviewsByReviewer(int reviewerId)
        {
            if(!_reviewerRepository.ReviewerExists(reviewerId))
                return NotFound();

            var reviews = _mapper.Map<List<ReviewerDto>>(_reviewerRepository.GetReviewsByReviewer(reviewerId));

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public IActionResult CreateReviewẻ([FromBody] ReviewerDto reviewerCreate)
        {
            if (reviewerCreate == null)
                return BadRequest(ModelState);

            var reviewer = _reviewerRepository.GetReviewers()
                .Where(c => c.FirstName.Trim().ToUpper() == reviewerCreate.FirstName.TrimEnd().ToUpper())
                .FirstOrDefault();

            if (reviewer != null)
            {
                ModelState.AddModelError("", "Reviewer already exists");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewerMap = _mapper.Map<Reviewer>(reviewerCreate);



            if (!_reviewerRepository.CreateReviewer(reviewerMap))
            {
                ModelState.AddModelError("", "Something went wrong while savin");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created");
        }

    }
}
