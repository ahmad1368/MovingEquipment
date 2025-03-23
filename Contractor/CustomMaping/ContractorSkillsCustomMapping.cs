using Contractor.Models;
using Entites;
using WebFramework.CustomMapping;

namespace Contractor.CustomMaping
{
    public class ContractorSkillsCustomMapping: IHaveCustomMapping
    {
        public void CreateMappings(AutoMapper.Profile profile)
        {
            profile.CreateMap<ContractorSkills, ContractorSkillsDTO>().ReverseMap();
        }
    }
    
}
