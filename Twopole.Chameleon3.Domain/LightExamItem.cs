using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoPole.Chameleon3.Domain
{
    public class LightExamItem 
    { 
        public int Id { get; set; }
        public LightExamItem()
        {
            UpdatedOn = DateTime.Now;
        }

        public virtual string GroupName { get; set; }
        public virtual string LightRules { get; set; }
        public virtual DateTime UpdatedOn { get; set; }
    }
}
