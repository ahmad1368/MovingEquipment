using AutoMapper;
using Contractor.Models;
using Entites;

namespace Contractor
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Skills, SkillDTO>().ReverseMap();
            CreateMap<User, UserDTO>().ReverseMap();
        }
    }
}
