
using Common.Exceptions;
using Common.Utilities;
using Contractor.Models;
using Data.Repositories;
using Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using WebFramework.Api;
using WebFramework.Filters;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using AutoMapper.QueryableExtensions;

namespace Contractor.Controllers
{
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class SkillController : ControllerBase
    {
        private ISkillRepository skillRepository;
        private UserManager<User> userManager;
        private readonly IMapper Mapper;


        public SkillController(ISkillRepository skillRepository, UserManager<User> userManager, IMapper mapper)
        {
            this.skillRepository = skillRepository;
            this.userManager = userManager;

            this.Mapper = mapper;
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<SkillDTO>>> Get()
        {
            //var Role = HttpContext.Skill.Identity.FindFirstValue(ClaimTypes.Role);

            var skills = await skillRepository.TableNoTracking.ProjectTo<SkillDTO>(Mapper.ConfigurationProvider)
                .ToListAsync();
            return skills;

           
           

        }

        [HttpGet("{Id:Guid}")]
        public async Task<ApiResult<Skills>> Get(Guid Id, CancellationToken cancellationToken)
        {
            var skill = await skillRepository.GetByIdAsync(cancellationToken, Id);
            if (skill == null)
            {
                throw new NotFoundException("Skill Not Found");
            }


            return skill;
        }



        [HttpPost]
        public async Task<ApiResult<Skills>> Create(SkillDTO skillDTO, CancellationToken cancellationToken)
        {
            //var user = await userManager.GetUserAsync(User); // Get current user
            //skillDTO.InsertUser = user.UserName;
            //skillDTO.InsertDate = DateTime.Now;
            //skillDTO.IsActive = true;

            //var NewSkill = Mapper.Map<Skills>(skillDTO);
            var NewSkill = skillDTO.ToEntity();


            await skillRepository.AddAsync(NewSkill, cancellationToken);
            return NewSkill;

        }

        [HttpPut]
        public async Task<ApiResult<Skills>> Update(SkillDTO skillDTO, CancellationToken cancellationToken)
        {

            var updateSkill = await skillRepository.GetByIdAsync(cancellationToken, skillDTO.Id);
            //var user = await userManager.GetUserAsync(User); // Get current user

            //updateSkill.UpdateDate = DateTime.Now;
            //updateSkill.UpdateUser = user.UserName;


            //Mapper.Map(skillDTO, updateSkill);
            updateSkill = skillDTO.ToEntity(updateSkill);

            await skillRepository.UpdateAsync(updateSkill, CancellationToken.None);
            return Ok(updateSkill);
        }
        [HttpDelete]
        public async Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {

            var skill = await skillRepository.GetByIdAsync(cancellationToken, id);
            await skillRepository.DeleteAsync(skill, CancellationToken.None);
            return Ok();
        }



    }
}
