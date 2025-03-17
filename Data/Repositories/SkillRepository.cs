using Common;
using Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class SkillRepository : Repository<Skills>, ISkillRepository, IScopedDependency
    {
        public SkillRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
