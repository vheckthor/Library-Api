using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using OurApi.models;

namespace OurApi.Controllers
{

    [ApiController]
    [Route("api/authors/{authorId}/courses")]
    public class CourseController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;

        public CourseController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            _courseLibraryRepository = courseLibraryRepository;
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet]
        public ActionResult<IEnumerable<CourseDto>> GetCoursesForAuthor(Guid authorId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var coures = _courseLibraryRepository.GetCourses(authorId);
            var mappedcourses = _mapper.Map<IEnumerable<CourseDto>>(coures);
            return Ok(mappedcourses);
        }


        [HttpGet("{courseId}",Name= "GetCourseForAuthor")]
        public ActionResult<CourseDto> GetCourseForAuthor(Guid authorId, Guid courseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound();
            }

            var coures = _courseLibraryRepository.GetCourse(authorId, courseId);
            if (coures == null)
            {
                return NotFound();
            }

            var mappedcourses = _mapper.Map<CourseDto>(coures);
            return Ok(mappedcourses);
        }


        [HttpPost]
        public ActionResult<CourseDto> CreateCoursesForAuthor(Guid authorId, [FromBody]CreateCourseDto createCourseDto)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound("Author does not exist please check and try again");
            }


            var createCourse = _mapper.Map<Course>(createCourseDto);
            _courseLibraryRepository.AddCourse(authorId, createCourse);
            var solution = _courseLibraryRepository.Save();
            var courseReturn = _mapper.Map<CourseDto>(createCourse);

           
            if (solution)
            {
                return CreatedAtRoute(
                    "GetCourseForAuthor",
                    new{
                        authorId=courseReturn.AuthorId,
                        courseId=courseReturn.Id
                    },
                    courseReturn);
               
            }

            return BadRequest("An error occurred unable to save changes");
        }

        [HttpPut("{courseId}")]
        public ActionResult UpdateCourseForAuthor(
            Guid authorId, 
            Guid courseId,
            CourseUpdateDto courseUpdateDto)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound("Author does not exist");
            }

            var courseFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseFromRepo == null)
            {
                //create a new entity (Upserting)
                var courseAddition = _mapper.Map<Course>(courseUpdateDto);
                courseAddition.Id = courseId;
                _courseLibraryRepository.AddCourse(authorId, courseAddition);
                _courseLibraryRepository.Save();
                var returner = _mapper.Map<CourseDto>(courseAddition);
                
                return CreatedAtRoute(
                    "GetCourseForAuthor",

                    new
                    {
                        authorId,
                        courseId = courseAddition.Id
                    }, returner);
            }

            _mapper.Map(courseUpdateDto, courseFromRepo);

            _courseLibraryRepository.UpdateCourse(courseFromRepo);
            _courseLibraryRepository.Save();
            return NoContent();
        }

        [HttpPatch("{courseId}")]
        public ActionResult PartiallyUpdatedCourseForAuthors
        (
            Guid authorId,
            Guid courseId,
            JsonPatchDocument<CourseUpdateDto> patchDocument
            )
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound("Unable to find author");
            }

            var courseFromRepo = _courseLibraryRepository.GetCourse(authorId, courseId);
            if (courseFromRepo == null)
            {
                var newCourse = new CourseUpdateDto();
                patchDocument.ApplyTo(newCourse);
                var courseToAdd = _mapper.Map<Course>(newCourse);
                courseToAdd.Id = courseId;

                _courseLibraryRepository.AddCourse(authorId,courseToAdd);
                _courseLibraryRepository.Save();
                var returner = _mapper.Map<CourseDto>(courseToAdd);
                return CreatedAtRoute("GetCourseForAuthor",
                    new {authorId, courseId = courseToAdd.Id},
                    courseToAdd);

                //return NotFound("Course not found");
            }

            var coursePatch = _mapper.Map<CourseUpdateDto>(courseFromRepo);
            
            patchDocument.ApplyTo(coursePatch,ModelState);
            if (!TryValidateModel(coursePatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(coursePatch, courseFromRepo);

            _courseLibraryRepository.UpdateCourse(courseFromRepo);
            _courseLibraryRepository.Save();
            return NoContent();

        }

        [HttpDelete("{courseId}")]
        public ActionResult DeleteCourse(Guid authorId,Guid courseId)
        {
            if (!_courseLibraryRepository.AuthorExists(authorId))
            {
                return NotFound("Unable to find author");
            }

            var collect = _courseLibraryRepository
                .GetCourse(authorId, courseId);

            if (collect == null)
            {
                return NotFound("The course cannot be found");
            }
            _courseLibraryRepository.DeleteCourse(collect);
            var result = _courseLibraryRepository.Save();
            if (result)
            {
                return Ok("Delete Successful");
            }

            return BadRequest("Unable to delete course");
        }
    }
}