using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OurApi.Extensions;
using OurApi.models;

namespace OurApi.Controllers
{
    
    [ApiController]
    [Route("api/authorcollections")]
    public class AuthorCollectionsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public AuthorCollectionsController(ICourseLibraryRepository courseLibraryRepository,IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository ?? throw new ArgumentException(nameof(courseLibraryRepository));
            _mapper = mapper ?? throw new ArgumentException(nameof(mapper));
        }

        [HttpGet("({ids})",Name = "GetAuthorsCollection")]
        public IActionResult GetAuthorsCollection(
            [FromRoute]
            [ModelBinder(BinderType = typeof(ArrayModelBinder))]
            IEnumerable<Guid> ids)
        {
            if (ids == null)
            {
                return BadRequest("Error in the input files");
            }

            var authorEntities = _courseLibraryRepository.GetAuthors(ids);
            if (ids.Count() != authorEntities.Count())
            {
                return NotFound("One or more authors cannot be found");

            }

            var authorsReturn = _mapper.Map<IEnumerable<AuthorDto>>(authorEntities);
            return Ok(authorsReturn);
        }


        [HttpPost]
        public ActionResult<IEnumerable<AuthorDto>> CreateCollectionAuthor(IEnumerable<CreateAuthorDto> authorCollection)
        {
            var authorReturn = _mapper.Map<IEnumerable<Author>>(authorCollection);

            foreach (var author in authorReturn)
            { 
                _courseLibraryRepository.AddAuthor(author);
            }
            bool ans=_courseLibraryRepository.Save();
            var collections = _mapper.Map<IEnumerable<AuthorDto>>(authorReturn);
            var rute = string.Join(",",authorReturn.Select(x => x.Id));
            if (ans)
            {
                return CreatedAtRoute(
                    "GetAuthorsCollection",
                    new{ids=rute},
                    collections);
            }

            return BadRequest("Unable to create authors");

        }
    }
}