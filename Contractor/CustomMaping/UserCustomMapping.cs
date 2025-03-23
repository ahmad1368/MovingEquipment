using Contractor.Models;
using Entites;
using WebFramework.CustomMapping;

namespace Contractor.CustomMaping
{
    public class UserCustomMapping: IHaveCustomMapping
    {
        public void CreateMappings(AutoMapper.Profile profile)
        {
            profile.CreateMap<User, UserDTO>().ReverseMap();
        }
    }
    
}
