using System;
using System.Collections.Generic;
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
        public User User { get; set; }
        public Skills Skill { get; set; }
    }
}
