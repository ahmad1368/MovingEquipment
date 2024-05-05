using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Entites
{
    public class User:BaseEntity<Guid>
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
        public int Age { get; set; }
        public int Gender { get; set; }
        public DateTimeOffset LastLoginDate { get; set; }

        

    }

   
}
