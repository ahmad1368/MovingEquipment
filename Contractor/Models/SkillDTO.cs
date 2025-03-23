using AutoMapper;
using Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using WebFramework.Api;
using WebFramework.CustomMapping;

namespace Contractor.Models
{
    public class SkillDTO : BaseDto<SkillDTO,Skills,Guid>,IHaveCustomMapping, IValidatableObject //BaseEntity<Guid> , IValidatableObject
    {

        public SkillDTO()
        {
                
        }

        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public int? Score { get; set; }

        public Guid? ParrentSkillsId { get; set; }

        /// <summary>
        /// For Test CustomMappings Method and No business
        /// </summary>
        public string CombineTitleScore { get; set; }
        public Skills? ParrentSkills { get; set; }
        public ICollection<Skills>? ChildSkills { get; set; }
        public ICollection<ContractorSkills>? ContractorSkills { get; set; }
        public override void CustomMappings(IMappingExpression<Skills, SkillDTO> mappingExperission)
        {
            // TODO: This function will call but CombineTitleScore is null allways
            mappingExperission.ForMember(
                des => des.CombineTitleScore,
                config => config.MapFrom(src => src.Title != null && src.Score.HasValue ? $"{src.Title} ({src.Score})" : "No Data" //$"{src.Title} ({src.Score})"

                )
                );
        }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title.Equals("test"))
                yield return new ValidationResult($"{nameof(Title)} Value is Not Valid", new[] { nameof(Title) });


        }
    }
   
}
