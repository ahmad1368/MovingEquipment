using Common.Utilities;
using Entites;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ApplicationDbContext:DbContext
    {
        public ApplicationDbContext(DbContextOptions options)
            :base(options)
        {
                
        }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        ////    optionsBuilder.UseSqlServer("Provider=SQLOLEDB.1;Integrated Security=SSPI;Persist Security Info=False;Initial Catalog=MovingEquipment;Data Source=.");
        //    optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=MovingEquipment;Integrated security=true");
        //    base.OnConfiguring(optionsBuilder);
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var entitesAssembly = typeof(IEntity).Assembly;
            modelBuilder.RegisterAllEntites<IEntity>(entitesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();
            modelBuilder.AddPluralizingTableNameConvention();

        }
    }
}
