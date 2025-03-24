using Contractor.Models;
using Data.Repositories;
using Entites;
using Microsoft.AspNetCore.Identity;
using System;
using WebFramework.Api;
using AutoMapper;

namespace Contractor.Controllers
{

    public class SkillController : CrudController<SkillDTO, SkillSelectDTO, Skills, Guid>
    {
        public SkillController(IRepository<Skills> repository, UserManager<User> userManager, IMapper mapper) : base(repository,userManager,mapper)
        {
            
        }
    }

    
}
