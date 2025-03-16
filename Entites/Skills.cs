using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites
{
    public class Skills : BaseEntity<Guid>
    {

        public string Title { get; set; }       

        public string? Description { get; set; }

        public int? Score { get; set; }

        public Guid? ParrentSkillsId { get; set; }
         
        public Skills? ParrentSkills { get; set; }
        public ICollection<Skills>? ChildSkills { get; set; }
        public ICollection<ContractorSkills>? ContractorSkills { get; set; }
    }

    public class SkillsConfiguration : IEntityTypeConfiguration<Skills>
    {
        public void Configure(EntityTypeBuilder<Skills> builder)
        {
            builder.Property(x => x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Description).HasMaxLength(500);
            builder.HasOne(x=>x.ParrentSkills).WithMany(x=>x.ChildSkills).HasForeignKey(x=>x.ParrentSkillsId);
            builder.HasMany(x => x.ContractorSkills).WithOne(p=>p.Skill).HasForeignKey(p=>p.SkillId);

        }
    }
}
