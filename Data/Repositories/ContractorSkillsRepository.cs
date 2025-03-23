using Common;
using Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
   public class ContractorSkillsDTORepository : Repository<ContractorSkills>, IContractorSkillRepository, IScopedDependency
    {
        public ContractorSkillsDTORepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
