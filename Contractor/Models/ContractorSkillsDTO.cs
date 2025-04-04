﻿using Entites;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using WebFramework.Api;
using WebFramework.CustomMapping;
using AutoMapper;

namespace Contractor.Models
{
    public class ContractorSkillsDTO : BaseDto<ContractorSkillsDTO, ContractorSkills, Guid>, IValidatableObject 
    {
       
        public string Title { get; set; }
        public string Descrition { get; set; }
        public double DollarPerHourForThisSkill { get; set; }
        public Guid UserId { get; set; }
        public string UserFullName { get; set; }
        public Guid SkillsId { get; set; }
        public string? SkillsTitle { get; set; }
        public string? SkillsDescription { get; set; }
        public string SkillContractTitle { get; set; }

       
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title.Equals("test"))
                yield return new ValidationResult($"{nameof(Title)} Value is Not Valid", new[] { nameof(Title) });
        }
    }
}
