using Common;
using Entites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories
{
    public class PostRepository : Repository<Posts>, IPostRepository, IScopedDependency
    {
        public PostRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
        }
    }
}
