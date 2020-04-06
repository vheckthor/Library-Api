using AutoMapper;
using CourseLibrary.API.Entities;
using OurApi.models;

namespace CourseLibrary.API.Profiles
{
    public class CourseProfile:Profile
    {
        public CourseProfile()
        {
            CreateMap<Course, CourseDto>();
            CreateMap<CreateCourseDto, Course>();
            CreateMap<CourseUpdateDto, Course>();
            CreateMap<Course, CourseUpdateDto>();
        }
    }
}