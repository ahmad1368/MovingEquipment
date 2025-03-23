using Entites;
using WebFramework.CustomMapping;

namespace Contractor.CustomMaping
{
    public class PostCustomMapping: IHaveCustomMapping
    {
        public void CreateMappings(AutoMapper.Profile profile)
        {
            //profile.CreateMap<Posts, PostDTO>().ReverseMap();
        }
    }
    
}
