using AutoMapper;
using Entites;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using WebFramework.Api;
using WebFramework.CustomMapping;

namespace Contractor.Models
{
    public class SkillSelectDTO : BaseDto<SkillSelectDTO, Skills, Guid>, IHaveCustomMapping
    {

        public SkillSelectDTO()
        {

        }

   
        public string Title { get; set; }

        public string? Description { get; set; }

        public int? Score { get; set; }

        /// <summary>
        /// For Test CustomMappings Method and No business
        /// </summary>
        public string CombineTitleScore { get; set; }
        public Skills? ParrentSkills { get; set; }
        public ICollection<Skills>? ChildSkills { get; set; }
        public ICollection<ContractorSkills>? ContractorSkills { get; set; }
        public override void CustomMappings(IMappingExpression<Skills, SkillSelectDTO> mappingExperission)
        {

            mappingExperission.ForMember(
                des => des.CombineTitleScore,
                config => config.MapFrom(src => src.Title != null && src.Score.HasValue ? $"{src.Title} ({src.Score})" : "No Data" //$"{src.Title} ({src.Score})"

                )
                );
        }
        
    }

}
