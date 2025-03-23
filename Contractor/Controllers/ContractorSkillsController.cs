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
    [Route("api/[controller]")]
    [ApiResultFilter]
    [ApiController]
    public class ContractorSkillsController : ControllerBase
    {
        private IContractorSkillRepository ContractorSkillRepository;
        private UserManager<User> UserManager;
        private readonly IMapper Mapper;
        public ContractorSkillsController(IContractorSkillRepository contractorSkillRepository, UserManager<User> userManager, IMapper mapper)
        {
            this.ContractorSkillRepository = contractorSkillRepository;
            this.UserManager = userManager;
            this.Mapper = mapper;
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ApiResult<List<ContractorSkillsDTO>>> Get()
        {
            //var Role = HttpContext.Skill.Identity.FindFirstValue(ClaimTypes.Role);

            var contractorSkills = await ContractorSkillRepository.TableNoTracking.ProjectTo<ContractorSkillsDTO>(Mapper.ConfigurationProvider)
                .ToListAsync();
            return contractorSkills;
        }

        [HttpGet("{id:Guid}")]
        public async Task<ApiResult<ContractorSkills>> Get(Guid id, CancellationToken cancellationToken)
        {
            var contractorSkills = await ContractorSkillRepository.GetByIdAsync(cancellationToken, id);
            if (contractorSkills == null)
            {
                throw new NotFoundException("Skill Not Found");
            }


            return contractorSkills;
        }



        [HttpPost]
        public async Task<ApiResult<ContractorSkills>> Create(ContractorSkillsDTO contractorSkillsDTO, CancellationToken cancellationToken)
        {
            //var user = await UserManager.GetUserAsync(User); // Get current user

            //contractorSkillsDTO.InsertUser = user.UserName;
            //contractorSkillsDTO.InsertDate = DateTime.Now;
            //contractorSkillsDTO.IsActive = true;

            //var NewSkill = Mapper.Map<ContractorSkills>(contractorSkillsDTO);
            var NewSkill = contractorSkillsDTO.ToEntity();

            await ContractorSkillRepository.AddAsync(NewSkill, cancellationToken);
            return NewSkill;

        }

        [HttpPut]
        public async Task<ApiResult<ContractorSkills>> Update(ContractorSkillsDTO contractorSkillsDTO, CancellationToken cancellationToken)
        {

            var updateContractSkills = await ContractorSkillRepository.GetByIdAsync(cancellationToken, contractorSkillsDTO.Id);
            //var user = await UserManager.GetUserAsync(User); // Get current user


            //updateContractSkills.UpdateDate = DateTime.Now;
            //updateContractSkills.UpdateUser = user.UserName;

            //Mapper.Map(contractorSkillsDTO, updateSkill);
            updateContractSkills = contractorSkillsDTO.ToEntity(updateContractSkills);


            await ContractorSkillRepository.UpdateAsync(updateContractSkills, CancellationToken.None);
            return Ok(updateContractSkills);
        }
        [HttpDelete]
        public async Task<ApiResult> Delete(Guid id, CancellationToken cancellationToken)
        {

            var skill = await ContractorSkillRepository.GetByIdAsync(cancellationToken, id);
            await ContractorSkillRepository.DeleteAsync(skill, CancellationToken.None);
            return Ok();
        }



    }
}
