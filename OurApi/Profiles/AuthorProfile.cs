using AutoMapper;
using OurApi.Extensions;
using OurApi.models;
namespace CourseLibrary.API.Profiles
{
    public class AuthorProfile:Profile
    {
        public AuthorProfile()
        {
            CreateMap<Entities.Author, AuthorDto>()
                .ForMember(
                    dest => dest.FullName,
                    opt => opt.MapFrom(sourceMember => $"{sourceMember.FirstName} {sourceMember.LastName}"))
                .ForMember(
                    dest => dest.Age, 
                    opt => opt.MapFrom(src => src.DateOfBirth.GetCurrentAge()));

            CreateMap<CreateAuthorDto, Entities.Author>();
        }
    }
}