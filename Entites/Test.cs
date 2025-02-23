using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites
{
    public class Test : BaseEntity<Guid>
    {
        public string Title { get; set; }

    }
}
