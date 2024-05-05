using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entites
{
    public class Skills:BaseEntity<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
       
        public int Score {  get; set; }
        public ContractorSkills? ParrentSkills { get; set; }
        public ICollection<ContractorSkills> ChildSkills { get; set; }
    }
}
