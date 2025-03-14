using Common.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



namespace Entites
{
    public class User : IdentityUser<Guid>, IEntity<Guid>
    {
        public User()
        {
            IsActive = true;
            SecurityStamp = Guid.NewGuid().ToString();
        }

        ////public string UserName { get; set; }        
        ////public string PasswordHash { get; set; }       
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        public int Age { get; set; }
        public bool IsActive { get; set; }
        public GenderType Gender { get; set; }
        public DateTimeOffset LastLoginDate { get; set; } 
        public ICollection<ContractorSkills>? ContractorSkills { get; set;}

    }

    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.Property(p => p.UserName).IsRequired().HasMaxLength(100);
            builder.Property(p=>p.PasswordHash).IsRequired().HasMaxLength(500);
            builder.Property(p => p.FullName).HasMaxLength(100);
            builder.HasMany(p=>p.ContractorSkills).WithOne(p=>p.User).HasForeignKey(p=>p.UserId);

        }
    }


}
