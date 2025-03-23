using Contractor.Models;
using Entites;
using WebFramework.CustomMapping;

namespace Contractor.CustomMaping
{
    public class SKillCustomMapping: IHaveCustomMapping
    {
        public void CreateMappings(AutoMapper.Profile profile)
        {
            profile.CreateMap<Skills, SkillDTO>().ReverseMap();
        }
    }
    
}
