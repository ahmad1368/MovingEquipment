using Entites;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Contractor.Models
{
    public class SkillDTO : BaseEntity<Guid> , IValidatableObject
    {
        [Required]
        public string Title { get; set; }

        public string? Description { get; set; }

        public int? Score { get; set; }

        public Guid? ParrentSkillsId { get; set; }

        public Skills? ParrentSkills { get; set; }
        public ICollection<Skills>? ChildSkills { get; set; }
        public ICollection<ContractorSkills>? ContractorSkills { get; set; }
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title.Equals("test"))
                yield return new ValidationResult($"{nameof(Title)} Value is Not Valid", new[] { nameof(Title) });

        }
    }
   
}
