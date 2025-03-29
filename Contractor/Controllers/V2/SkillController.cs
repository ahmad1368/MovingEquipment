using Contractor.Models;
using Data.Repositories;
using Entites;
using Microsoft.AspNetCore.Identity;
using System;
using WebFramework.Api;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;
using System.Threading.Tasks;
using System.Collections.Generic;
using Common.Exceptions;

namespace Contractor.Controllers.V2
{

    [ApiVersion("2.0")]
    public class SkillController : V1.SkillController
    {
        public SkillController(IRepository<Skills> repository, UserManager<User> userManager, IMapper mapper) : base(repository,userManager,mapper)
        {

            
            
        }

        [HttpGet("{Id}")]
        public override async Task<ApiResult<SkillSelectDTO>> Get(Guid Id, System.Threading.CancellationToken cancellationToken)
        {

            throw new NotFoundException("Get Method Version 2");



        }


    }

    
}
