using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.input;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurApi.Extensions;
using OurApi.models;

namespace OurApi.Controllers
{

    [ApiController]
    [Route("api/authors")]
    //[Route("api/authors")]
    //[Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorsController(ICourseLibraryRepository courseLibraryRepository,IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository;
            _mapper = mapper?? throw new ArgumentNullException(nameof(mapper));
        }
        
        [HttpGet()]
        [HttpHead]
        public IActionResult GetAuthors([FromQuery] Queries queries)
        {
            var authorsFromTheRepo = _courseLibraryRepository
                .GetAuthors(queries);

            if (authorsFromTheRepo.Count() == 0)
            {
                return StatusCode(404, "There is no Result found");
            }  
            
            var authors = _mapper.Map<IEnumerable<AuthorDto>>(authorsFromTheRepo);

            
            return Ok(authors);
            

        }

        [HttpGet("{authorId}")]
        public IActionResult GetAuthor(Guid authorId)
        {

            var singleAuthor = _courseLibraryRepository.GetAuthor(authorId);

            if (singleAuthor==null)
            {
                return NotFound("No result found");
            }
            var singleAuthorDto= _mapper.Map<AuthorDto>(singleAuthor);
            

            return Ok(singleAuthorDto);
        }

        [HttpPost()]
        public IActionResult CreateAuthor([FromBody]CreateAuthorDto author)
        {
            var create = _mapper.Map<Author>(author);
             _courseLibraryRepository.AddAuthor(create);
             var ans = _courseLibraryRepository.Save();
           //_courseLibraryRepository.Save();
            if (ans)
            {
                var authorReturn = _mapper.Map<AuthorDto>(create);
                return CreatedAtAction(
                    "GetAuthor",
                    new { authorId = authorReturn.Id },
                    authorReturn);
                
            }

            return BadRequest("Unable to save changes");
        }

        [HttpOptions]
        public IActionResult AuthorsOptions()
        {
            Response.Headers.Add("Allow","GET,POST,OPTIONS");
            return Ok();
        }

        [HttpDelete("{authorId}")]
        public ActionResult DeleteAuthor(Guid authorId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound("Author does not exist");
            }

            var auth = _courseLibraryRepository.GetAuthor(authorId);
            if (auth == null)
            {
                return NotFound("Author cannot be found");
            }
            _courseLibraryRepository.DeleteAuthor(auth);
            var result = _courseLibraryRepository.Save();
            if (result)
            {
                return Ok("Author has been deleted Successfully");
            }

            return BadRequest("Unable to delete author");
        }
    }
}