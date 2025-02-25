using Common.Utilities;
using Entites;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Contractor.Models
{
    public class UserDTO : IValidatableObject
    {
        [Required]
        [StringLength(100)]
        public string UserName { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; }

        [Required]
        public int Age { get; set; }
        [Required]
        public GenderType Gender { get; set; }

        [Required]
        public string Password { get; set; }
        

       public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (UserName.Equals("test", StringComparison.OrdinalIgnoreCase))
                yield return new ValidationResult($"{nameof(UserName)} Can't be test", new[] { nameof(UserName) });
            if (Password.Equals("123456"))
                yield return new ValidationResult($"{nameof(Password)} Value is Not Valid", new[] { nameof(Password) });
            if (Gender == GenderType.Male && Age > 100 )
                yield return new ValidationResult("Mens cant be older than 100", new[] { nameof(Gender), nameof(Age) });

        }
    }

}
