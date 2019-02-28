using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace TwoPole.Chameleon3.Domain
{
    public class VehicleModel 
    {
        public VehicleModel()
        {
            GearSettings = new HashSet<VehicleModelGearSetting>();
        }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Brand { get; set; }
        public string Remark { get; set; }
        public virtual ISet<VehicleModelGearSetting> GearSettings { get; set; }
    }
}
