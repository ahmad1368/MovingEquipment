using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites
{
    public class ContractorSkills: BaseEntity<Guid>
    {       

        public string Title { get; set; }
        public string Descrition { get; set; }
        public double DollarPerHourForThisSkill { get; set; }
        public Guid UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public Guid SkillsId { get; set; }
        [ForeignKey(nameof(SkillsId))]
        public Skills Skills { get; set; }
    }
    public class ContractorSkillsConfiguration : IEntityTypeConfiguration<ContractorSkills>
    {
        public void Configure(EntityTypeBuilder<ContractorSkills> builder)
        {
            builder.Property(p => p.Title).IsRequired().HasMaxLength(100);
            builder.Property(p => p.Descrition).IsRequired().HasMaxLength(500);
            builder.HasOne(p=>p.User).WithMany(p=>p.ContractorSkills).HasForeignKey(p=>p.UserId);
            builder.HasOne(p=>p.Skills).WithMany(p=>p.ContractorSkills).HasForeignKey(p=>p.SkillsId);
        }
    }
}
