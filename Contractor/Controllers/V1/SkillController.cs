using Contractor.Models;
using Data.Repositories;
using Entites;
using Microsoft.AspNetCore.Identity;
using System;
using WebFramework.Api;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Common.Exceptions;
using WebFramework.Filters;

namespace Contractor.Controllers.V1
{

    [ApiVersion("1.0")]
    public class SkillController : CrudController<SkillDTO, SkillSelectDTO, Skills, Guid>
    {
        public SkillController(IRepository<Skills> repository, UserManager<User> userManager, IMapper mapper) : base(repository, userManager, mapper)
        {

        }

        [HttpGet("{Id}")]
        public override async Task<ApiResult<SkillSelectDTO>> Get(Guid Id, System.Threading.CancellationToken cancellationToken)
        {

            throw new NotFoundException("Get Method Version 1");



        }
    }
}
