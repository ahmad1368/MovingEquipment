using Common.Utilities;
using Entites;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Contractor.Models
{
    public class UserDTO
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
        public string Password { get;  set; }
    }
}
