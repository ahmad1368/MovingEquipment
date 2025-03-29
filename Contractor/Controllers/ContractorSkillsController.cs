using AutoMapper;
using Common.Exceptions;
using Contractor.Models;
using Data.Repositories;
using Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using WebFramework.Api;
using WebFramework.Filters;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace Contractor.Controllers
{

    public class ContractorSkillController : CrudController<ContractorSkillsDTO,ContractorSkillsSelectDTO, ContractorSkills, Guid>
    {
        public ContractorSkillController(IRepository<ContractorSkills> repository, UserManager<User> userManager, IMapper mapper) : base(repository, userManager, mapper)
        {

        }
    }

    
}
